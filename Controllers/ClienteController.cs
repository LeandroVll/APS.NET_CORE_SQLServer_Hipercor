using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EO.Internal;
using FluentEmail.Core;
using FluentEmail.Mailgun;
using HipercoreASPNETCORE.Interfaces;
using HipercoreASPNETCORE.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace HipercoreASPNETCORE.Controllers
{
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] //le indico el tipo de schema de autorizacion
    public class ClienteController : Controller
    {
        private IServicioAccesoBD _accesoBD;
        private IServicioEnvioEmail _enviarEmail;
        private IConfiguration _configuracion;

        #region  ".....propiedades de clase....."


        #endregion

        //---------------------------------------------------------------------------------

        #region ".....metodos de clase ...."

            //cosntructor del controlador q mapea mis servicios en variables privadas 
            public ClienteController(IServicioAccesoBD servicioAccesoBD, IServicioEnvioEmail servicioEnvioEmail, IConfiguration configuration)
            {                                                                
                this._accesoBD = servicioAccesoBD; //servicioAccesoDB es creado por el modulo de iyenccion de dependecias  y se mapea en la variable privada

                this._enviarEmail = servicioEnvioEmail; //servicio de envio de emails

                this._configuracion = configuration; //interfaz q permite usar variables de entorno
            }

           

        #region ".....metodos de clase que originan vistas al cleint...."

        //-------------------------------registro del cleinte -----------------------------

        [HttpGet]
            public IActionResult Registro()
        {
                return View();
        }

        [HttpPost]
            public IActionResult Registro(Cliente NewCliente) //le paso un obj cliente
        {

             if (ModelState.GetValidationState("Nombre") == ModelValidationState.Invalid  || 
                  ModelState.GetValidationState("PrimerApellido") == ModelValidationState.Invalid ||
                  ModelState.GetValidationState("SegundoApellido") == ModelValidationState.Invalid ||
                  ModelState.GetValidationState("Email") == ModelValidationState.Invalid ||
                  ModelState.GetValidationState("Password") == ModelValidationState.Invalid ||
                  ModelState.GetValidationState("RePassword") == ModelValidationState.Invalid)
             {
                  return View(NewCliente);//<--devuelve a la vista el obj cliente porq es erroneo
              }
              else 
              {
                //Aqui se haria la insercion de los datos en la BBDD
                //se asigan un valor por defecto a DNI porque es required por la BD
                NewCliente.NIF = "00000000-A";
                NewCliente.Telefonofijo = "912345678910";
                NewCliente.TelefonoMovil = "612345678";
                int _filasRegitradas = this._accesoBD.RegistrarCliente(NewCliente);
                //la variable almacena el numero decolumnas q devuleve la BD 1 = ok , 0 = error
                
                if (_filasRegitradas == 1)
                {
                    //se envia correo de verificacion 
                    String _destino = NewCliente.CredencialesAcceso.Email; //<--guradar el valor del email del formulario 
                    var _url = Url.Action("ActivarCuenta", "Cliente", new { email = _destino}, protocol: HttpContext.Request.Scheme);
                    String _asunto="Confirmacion cuenta HIPERCOR "+ NewCliente.Nombre;
                    String _cuerpoHTML = $"por favor, confirma tu registro pulsando el link de abajo <br/> {_url}";


                    Boolean _envioEmaiOK = this._enviarEmail.EnvioDeEmail(_destino,_asunto,_cuerpoHTML); //llamo al sevicio envioemail
                    if (_envioEmaiOK)
                    {

                        return View("Login");
                    }
                    else 
                    {
                        return RedirectToAction("ServidorError");
                    }
                }
                else
                {
                    //Mandar mensaje de error personalizado 
                    ModelState.AddModelError("", "ERROR INTERNO DEL SERVIDOR, intenetelo mas tarde...");
                    return View(NewCliente);//<--devuelve a la vista el obj cliente porq es erroneo
                }


               
              }
            
        }

        #endregion

        #endregion

        //----------------------------------------------- Metodo para el login ----------------------------------

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Credenciales _credemcialesRegistro)
        {

            if (!ModelState.IsValid)
            {
                return View(_credemcialesRegistro);
            }
            else 
            {
                //comprobar q existe en la BD un cleinte con estas credenciales
                Boolean _resultadoLogin = this._accesoBD.ComprobarCredenciales(_credemcialesRegistro.Email, _credemcialesRegistro.Password);
                if (_resultadoLogin)
                {
                    //aqui se llama al metodo contructor de tokens y devuelve el token al navegador
                    //como lo devuelvo en el return justo con la vista??
                    var oToken= BuildToken(_credemcialesRegistro);
                    return View("Tienda");
                }
                else 
                {
                    ModelState.AddModelError("", ".....Password o Email invalidos....");
                    return View(_credemcialesRegistro);
                }

            }

        }


        //----------------------------------------------- Metodo para Activar Cuenta ----------------------------------

        [HttpGet]
        public IActionResult ActivarCuenta(string emialRecibido)
        {
            string email = HttpContext.Request.Query["email"].ToString();
           // string emailRecibido = Request.Form["email"].ToString();

            // EmailRecibido = email;
            Boolean _filasRegitradas = this._accesoBD.ActivarCuenta(email);
            if (_filasRegitradas)
            {
                return View("Login");//<---aqui redirigir la vista 
            }
            return View("PaginaError");
        }

        [HttpPost]
        public IActionResult ActivarCuenta()
        {
            if (true) { return View(); }
           

        }


        //----------------------------------------------- Metodo de creacion de JWT ---------------------------------------

        private UserToken BuildToken(Credenciales credenciales)
        {
            var Claims = new[] //<---array de identificadores del jwt
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, credenciales.Email), //constructor al q le paso un valor unico
                new Claim(JwtRegisteredClaimNames.UniqueName, credenciales.Password),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())   //genera un string unico como Id para el token 

            };

            //lave de seguridad , se le pasa un array de bits 
            //con Encoding.UTF8 paso un string (la cadena aleatoria de lo q se quiera en una var de entorno) a un [] de bits 
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuracion["JWT_Security_Key"]));

            //declaro las credenciales q le voy a pasar (key, algooritmo de seguridad jwt) 
            var Credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var _expiration = DateTime.UtcNow.AddHours(1); //<---tiempo de validez del token 
            //creo el token con los campos anteriores 
            //instacio la variable token del tipo JWTSecurity
            JwtSecurityToken _Token = new JwtSecurityToken
                (            
                    issuer: "",
                    audience: "",
                    expires: _expiration,
                    signingCredentials: Credentials
                );
            //devuelvo el token           
            return new UserToken()
            {
                token = new JwtSecurityTokenHandler().WriteToken(_Token),
                expiration = _expiration
            };
        }

    }
}