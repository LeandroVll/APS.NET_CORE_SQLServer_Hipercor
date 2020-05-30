using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using HipercoreASPNETCORE.Interfaces;
using HipercoreASPNETCORE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace HipercoreASPNETCORE.Controllers
{
    [JsonArray]
    public class TiendaController : Controller
    {
        //variables privadas
        private IServicioAccesoBD _accesoBD;
        private IServicioEnvioEmail _enviarEmail;
        private IConfiguration _configuracion;

        #region  ".....propiedades de clase....."


        #endregion

        //---------------------------------------------------------------------------------

        #region ".....metodos de clase ...."
        public TiendaController(IServicioAccesoBD servicioAccesoBD, IServicioEnvioEmail servicioEnvioEmail, IConfiguration configuration)
        {
            this._accesoBD = servicioAccesoBD; //servicioAccesoDB es creado por el modulo de iyenccion de dependecias  y se mapea en la variable privada

            this._enviarEmail = servicioEnvioEmail; //servicio de envio de emails

            this._configuracion = configuration; //interfaz q permite usar variables de entorno
        }
        #endregion

        //--------
        #region ".....metodos de clase que originan vistas de la tienda...."

        
        //-----------------------------------------------------------------------------------
        [HttpGet]
        public IActionResult DatosDeEnvio()
        {
            //si vuelve a esta pagina da error 
            var _sessionCliente = JsonConvert.DeserializeObject<Cliente>(HttpContext.Session.GetString("sesionCliente"));
            return View("FormDireccion");//<---llega por url desde el panel de usuario y redirige a la vista formulario de envio
        }
        [HttpPost]
        public IActionResult DatosDeEnvio([FromForm]Direccion _Direccion) //<--recibe los datos del form de datosdeEnvio
        {
            //recojo la session
            var _sessionCliente = JsonConvert.DeserializeObject<Cliente>(HttpContext.Session.GetString("sesionCliente"));
            //--el cliente entra en su panel y elije la opcion libreta de direcciones 
            //  se le redirije a la vista Direcciones
            if (ModelState.GetValidationState("TipoVia") ==ModelValidationState.Invalid ||
                ModelState.GetValidationState("NombreVia") == ModelValidationState.Invalid ||
                ModelState.GetValidationState("CP") == ModelValidationState.Invalid ||
                ModelState.GetValidationState("CodMuni") == ModelValidationState.Invalid ||
                ModelState.GetValidationState("NomMunicipio") == ModelValidationState.Invalid ||
                ModelState.GetValidationState("CodPro") == ModelValidationState.Invalid ||
                ModelState.GetValidationState("NomProvincia") == ModelValidationState.Invalid ||
                ModelState.GetValidationState("Edificio") == ModelValidationState.Invalid ||
                ModelState.GetValidationState("Escalera") == ModelValidationState.Invalid ||
                ModelState.GetValidationState("Piso") == ModelValidationState.Invalid ||
                ModelState.GetValidationState("Puerta") == ModelValidationState.Invalid 
                ) 
             {
                 return View("FormDireccion");
             }
            else
            {
                //------esto es un redirect to action y pasarlo a otro iaction
                //tengo q meter en el array del obj cliente el obj direccion
                _sessionCliente.Direcciones.Add(_Direccion);
                //para buscar el usuario e insertar los datos direccion del formulario tengo q pasarle el obj cleinte
                //este ya tiene el campo direccion, q si es nuevo viene null..o vacio                
                int _filasRegistradas = this._accesoBD.RegistrarDireccion(_sessionCliente);
                if (_filasRegistradas!=1)
                {

                    return RedirectToAction("Productos");//<--si la insecion de la direccion ha ido bien redirije al actionresut
                                                            //que llama a todos los productos y los muestra en la vista Tienda
                }
                else
                {
                    return RedirectToAction("ServidorError");
                }

            }


        }

        //----------devuelve un arary de obj a la vista con el mismo nombre
        public IActionResult Productos()
        {

            List<Producto> productos = this._accesoBD.TodosLosProductos();
           
            return View(productos);//<--pasa a la vsta el array de productos 

        }

        //[HttpGet]
       
        public IActionResult AgregaraCarrito(int idProducto)
        {

            var _sessionCliente = JsonConvert.DeserializeObject<Cliente>(HttpContext.Session.GetString("sesionCliente"));
           
            List<Pedido> _sessionListaCarrito;
            int cantidad = 0;
            //---variables q sobreescriben los datos del pedido
            Pedido unPedido;
            Producto esteProducto;
            decimal precioUnidad;
            decimal subTotal;
            decimal gastosEnvio;

            Producto _producto = this._accesoBD.DevolverProducto(idProducto);//<---busco el producto con el id 
            string sCarrito = (HttpContext.Session.GetString("sesionCarrito"));//<--deserializar primero, sino da error json
            if (sCarrito != null) //<---si hay hay un obj peidido en la _sessionCarrito q se añada al diccionario el nuevo producto
            {
                
                _sessionListaCarrito = JsonConvert.DeserializeObject<List<Pedido>>(sCarrito);//<--no se puede pasar como string 

                if (_sessionListaCarrito.Any())
                {
                    foreach (Pedido pedidoExistente in _sessionListaCarrito) //<--recorro el array de pedidos 
                    {
                        //si el valor int de esta key (producto) es distinto de 0 existe el producto en el diccionario
                        if (pedidoExistente.ListaElementosPedidos.ContainsKey(_producto.idProducto))
                        {
                            //guardo el valor para la key (_idProducto.id) y le sumo 1 
                            cantidad = pedidoExistente.ListaElementosPedidos[_producto.idProducto]; 
                            pedidoExistente.ListaElementosPedidos[_producto.idProducto] = cantidad + 1;
                        }
                        else //<---si el producto no está en el diccionario q se añada al pedido ya existente
                        {
                            pedidoExistente.ListaElementosPedidos.Add(_producto.idProducto, 1);
                        }

                    }
                }
                else //si no hay datos en el _sessionCarrito se crea un obj newPedido y se guarda en el diccionario el producto seleccionado
                {
                    
                    Random rnd = new Random();
                    int idpedido = rnd.Next(1, 100000000);

                    Pedido newPedido = new Pedido(); 

                    //creo el pedido con algunos valores por defecto q se insertaran el la tabla y luego se modificaran
                    newPedido.idPedido = idpedido.ToString();
                    newPedido.NifCliente = _sessionCliente.NIF;
                    newPedido.FechaPedido = DateTime.Now;
                    newPedido.EstadoPedido = "En proceso de compra";
                    newPedido.ListaElementosPedidos = new Dictionary<string, int>();
                    newPedido.ListaElementosPedidos.Add(_producto.idProducto, 1); //<--el int es cantidad del mismo producto
                    newPedido.TipoGastosEnvio = "Envio estandar" ;
                    newPedido.GastosEnvio=5;
                    newPedido.SubTotalPedido = 0;
                    newPedido.TotalPedido = 0;
                    //se modifican los datos relacionados con el dinero suma de total...
                    foreach (var unproducto in newPedido.ListaElementosPedidos) //<---desgloso el dictionary q contiene la id de cada prodcuto y su cantidad 
                    {
                        esteProducto = this._accesoBD.DevolverProducto(Int32.Parse(_producto.idProducto)); //<---busco el producto en la BD 

                        //multiplico por cantidades (value de la key)
                        precioUnidad = esteProducto.PrecioProducto;
                        subTotal = precioUnidad * (unproducto.Value);
                        newPedido.SubTotalPedido = subTotal;

                        //el total es la suma de del subtotal y los gastos de envio
                        gastosEnvio = newPedido.GastosEnvio;
                        newPedido.TotalPedido = subTotal + gastosEnvio;
                    }

                    _sessionListaCarrito.Add(newPedido);

                }

                HttpContext.Session.SetString("sesionCarrito", JsonConvert.SerializeObject(_sessionListaCarrito));//<---paso lista como sesion
                
            }

            
            return RedirectToAction("Productos"); //<--q vuelva a la vista para seguir comprando
        }


        public IActionResult GuardarPedido()
        {

            var _sessionCliente = JsonConvert.DeserializeObject<Cliente>(HttpContext.Session.GetString("sesionCliente"));
            string sCarrito = (HttpContext.Session.GetString("sesionCarrito"));
            List<Pedido> _sessionListaCarrito;
            if (sCarrito != null) //<---si hay hay un obj peidido en la _sessionCarrito (json)
            {

                _sessionListaCarrito = JsonConvert.DeserializeObject<List<Pedido>>(sCarrito);
                //deserializar pasar a un obj pedido y guardar en la BD 
                // inserto el pedido en la BD
                Pedido newPedido;
                foreach (Pedido pedidoExistente in _sessionListaCarrito) //<--recorro el array de pedidos 
                {

                    //creo el obj pedido a para guardar los datos de la sesionCarrito 
                    newPedido = new Pedido();
                    newPedido.idPedido = pedidoExistente.idPedido;
                    newPedido.NifCliente = pedidoExistente.NifCliente;
                    newPedido.FechaPedido = pedidoExistente.FechaPedido;
                    newPedido.EstadoPedido = pedidoExistente.EstadoPedido;
                    newPedido.TipoGastosEnvio = pedidoExistente.TipoGastosEnvio;
                    newPedido.GastosEnvio = pedidoExistente.GastosEnvio;
                    newPedido.SubTotalPedido = pedidoExistente.SubTotalPedido;
                    newPedido.TotalPedido = pedidoExistente.TotalPedido;
                    newPedido.ListaElementosPedidos = pedidoExistente.ListaElementosPedidos;

                    Boolean PedidoGuardado = this._accesoBD.InsertarPedido(newPedido); //<---guardo el pedido
                }

            }

            return RedirectToAction("GestionarPedido");
        }



    //----------------------------------------------------ver pedidos-------------------------------------------------
        public IActionResult GestionarPedido()
        {
            //despliega los datos de los pedidos asociados al nif del usuario usuario
            //se muestra 
            //recojo la session
            var _sessionCliente = JsonConvert.DeserializeObject<Cliente>(HttpContext.Session.GetString("sesionCliente"));
            List<Pedido> PedidosList = this._accesoBD.DevolverPedisosList(_sessionCliente.NIF); //<--bsuca los pedidos del cleinte 
            List<Producto> ProductosList= new List<Producto>();
            Escaparate objEscaparate = new Escaparate();
            //---variables q sobreescriben los datos del pedido
            Pedido unPedido;
            Producto esteProducto;
            decimal precioUnidad;
            decimal subTotal;
            decimal gastosEnvio;

            if (PedidosList.Any())//<---si hay pedidos asociados a ese cliente
            {

                //---aqui hay q modificar los datos antes de mostrarlos: sumar productos + subtotal = total

                foreach (Pedido item in PedidosList) //<--desgloso cada pedido de la lista
                {
                   
                    unPedido = new Pedido();
                    unPedido.idPedido = item.idPedido;
                    unPedido.NifCliente = item.NifCliente;
                    unPedido.FechaPedido = item.FechaPedido;
                    unPedido.EstadoPedido = item.EstadoPedido;
                    unPedido.TipoGastosEnvio = item.TipoGastosEnvio;
                    unPedido.GastosEnvio = item.GastosEnvio;
                    unPedido.SubTotalPedido = item.SubTotalPedido; //<---se multiplician productos por precio y se sobreescribe
                    unPedido.TotalPedido = item.TotalPedido;    //<----
                    unPedido.ListaElementosPedidos = item.ListaElementosPedidos; 

                    foreach (var unproducto in unPedido.ListaElementosPedidos) //<---desgloso el dictionary q contiene la id de cada prodcuto y su cantidad 
                    {
                        esteProducto = this._accesoBD.DevolverProducto(Int32.Parse(unproducto.Key)); //<---busco el producto en la BD 

                        //multiplico por cantidades (value de la key)
                        precioUnidad = esteProducto.PrecioProducto;
                        subTotal = precioUnidad * (unproducto.Value);
                        unPedido.SubTotalPedido = subTotal;

                        //el total es la suma de del subtotal y los gastos de envio
                        gastosEnvio = unPedido.GastosEnvio;
                        unPedido.TotalPedido = subTotal + gastosEnvio;

                        ProductosList.Add(esteProducto); //<---guardo cada producto para mostrarlo en la vista
                    }

                    //una vez modificado, Update la tabla de la BD
                    Boolean ActualizarPedido = this._accesoBD.ActualizarPedido(unPedido);

                }

                objEscaparate.ListaDatosProducto = ProductosList;
                objEscaparate.ListDatosPedido = PedidosList;
                return View(objEscaparate); //<---devuelve a la vsta Productos la lista de pedidos de la BD
            }

            return View("SinPedidos");        
        
        
        }


        //--------------------------------confirmar compra mandando un mail -----------------------------------------

        public IActionResult ConfirmarCompra(string idPedido)
        {
            var _sessionCliente = JsonConvert.DeserializeObject<Cliente>(HttpContext.Session.GetString("sesionCliente"));
            //recivo el id del pedido y paso un email con el resumen del pedido
            String estado = "Pendiente de Envio";
            Pedido unPedidoCliente = this._accesoBD.DevolverUnPedido(idPedido); //<---buscar el pedido por su id
          //  unPedidoCliente.ListaElementosPedidos= unPedidoCliente.ListaElementosPedidos; //
            Boolean EstadoPedido = this._accesoBD.EstadodelEnvio(idPedido, estado); //<---actualiza el estado del pedido
            List<Producto> losProductosPedidos = new List<Producto>();
            
            //---variables del mail
            Producto esteProducto;
            List<string> datosHTML=new List<string>();
            String _cuerpoHTML1 = "";
            String _cuerpoHTML2 = "";
            foreach (var unproducto in unPedidoCliente.ListaElementosPedidos) //<---recorro el dictionary 
            {
                esteProducto = this._accesoBD.DevolverProducto(Int32.Parse(unproducto.Key)); //<---busco el producto en la BD                 
                losProductosPedidos.Add(esteProducto);
            }

            foreach (Producto item in losProductosPedidos)
            {
                _cuerpoHTML1 = $"<li>" +
                                 "Producto: " + item.NombreProducto + " Precio " + item.PrecioProducto + "  ud"
                            + "<li>"+
                            "</br>"  ;
                datosHTML.Add(_cuerpoHTML1);
            }
            _cuerpoHTML2 = "<b>Precio Total:"+ unPedidoCliente.TotalPedido +"</b>" +
                             "Estado del Pedido:" + unPedidoCliente.EstadoPedido;
            //se envia correo con el desglose
            String _destino = _sessionCliente.CredencialesAcceso.Email; //<--email cliente 
            String _asunto = "Desglose su Pedido " + _sessionCliente.Nombre;

            //****falta crear la string con cada dato sin q sobre escriba

            Boolean _envioEmaiOK = this._enviarEmail.EnvioDeEmail(_destino, _asunto, _cuerpoHTML1+_cuerpoHTML2); //llamo al sevicio envioemail
            if (_envioEmaiOK)
            {
                return RedirectToAction("CompraConfirmada");
            }

            return View("PaginaError");

        }

        //------------------------------------------eliminar pedido---------------------------------------------------
        public IActionResult CancelarPedido(string idPedido)
        {
            var _sessionCliente = JsonConvert.DeserializeObject<Cliente>(HttpContext.Session.GetString("sesionCliente"));
            Boolean PedidoEliminado = this._accesoBD.EliminarUnPedido(idPedido); //<---buscar y eliminar el pedido por su id
            if (PedidoEliminado)
            {
                return RedirectToAction("PaneldeUsuario", "Cliente");
            }
            return View("PaginaError");

        }


        //--------------------------------------vista de confimacion -----------------------------------------
        public IActionResult CompraConfirmada()
        {

            return View("CompraConfirmada");
        
        }

        //-------------------------muesta los datos del producto seleccionado en el carrito--------------------------
 
        public IActionResult DetalleProducto(string idProducto)
        {

            var _sessionCliente = JsonConvert.DeserializeObject<Cliente>(HttpContext.Session.GetString("sesionCliente"));

            Producto esteProducto = this._accesoBD.DevolverProducto(Int32.Parse(idProducto)); //<---busco el producto en la BD 

            return View(esteProducto);

        }

        //----------------------------suma uno al value del diccionario q es la cantidad de un producto----------------------
        public IActionResult ModificarCantidad(string idProducto, string idPedido)
        {

            var _sessionCliente = JsonConvert.DeserializeObject<Cliente>(HttpContext.Session.GetString("sesionCliente"));

            Pedido PedidoModificar = this._accesoBD.DevolverUnPedido(idPedido); //<--devulve el predido por id 

            Producto elPrducto= new Producto();
            int cantidad = 0;

            //si el valor int de esta key (producto) es distinto de 0 existe el producto en el diccionario
                if (PedidoModificar.ListaElementosPedidos.ContainsKey(idProducto))
                {
                    //guardo el valor para la key (_idProducto.id) y le sumo 1 
                    cantidad = PedidoModificar.ListaElementosPedidos[idProducto];
                    PedidoModificar.ListaElementosPedidos[idProducto] = cantidad + 1;
                }
                else //<---si el producto no está en el diccionario q se añada al pedido ya existente
                {
                    PedidoModificar.ListaElementosPedidos.Add(idProducto, 1);
                }
            //guardo el peddo modificado 
            Boolean Pedidomodificado = this._accesoBD.ModificarPedidoEntero(PedidoModificar);
            if (Pedidomodificado)
            {
                return RedirectToAction("GestionarPedido");
            }

            return RedirectToAction("Error");

        }

        //---------------------------------eliminar producto del pedido----------------------------------
        public IActionResult EliminarProductoPedido(string idProducto, string idPedido)
        {
            var _sessionCliente = JsonConvert.DeserializeObject<Cliente>(HttpContext.Session.GetString("sesionCliente"));

            Pedido PedidoModificar = this._accesoBD.DevolverUnPedido(idPedido); //<--devulve el predido por id 

            Producto elPrducto = new Producto();
            int cantidad = 0;

            //si el valor int de esta key (producto) es distinto de 0 existe el producto en el diccionario
            if (PedidoModificar.ListaElementosPedidos.ContainsKey(idProducto))
            {
                //guardo el valor para la key (_idProducto.id) y le sumo 1 
                cantidad = PedidoModificar.ListaElementosPedidos[idProducto];
                if (cantidad > 1)                                              //<---cantidad minima del producto en el pedido (1)
                {
                    PedidoModificar.ListaElementosPedidos[idProducto] = cantidad - 1;

                }        
            }
            else //<---si el producto no está en el diccionario q se añada al pedido ya existente
            {
                PedidoModificar.ListaElementosPedidos.Add(idProducto, 1);
            }
            //guardo el peddo modificado 
            Boolean Pedidomodificado = this._accesoBD.ModificarPedidoEntero(PedidoModificar);
            if (Pedidomodificado)
            {
                return RedirectToAction("GestionarPedido");
            }

            return RedirectToAction("Error");

        }



        //--------------------------------------AÑADE MEDIOS DE PAGO -----------------------------------------
        public IActionResult MediodePago()
        {

            return View("MediodePago");

        }


















        #endregion

    }
}