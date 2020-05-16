using HipercoreASPNETCORE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//Espacio de nombres relacionados, necesarios para hacer operaciones coontra sqlServer
using System.Data.SqlClient;
using System.Data;


namespace HipercoreASPNETCORE.Models
{

   
    public class ServicioaccesoDBSQLServer : IServicioAccesoBD
    {


         String _cadenaConexionBD = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=HipercorDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        #region "...Acceso a las tablas relcionadas con el CLIENTE


        public bool ComprobarCredenciales(string _emailcliente, string _password)
        {

            try 
            {

                //---aqui debo hacer el login 
                //se abre conexion a la bbdd con la conexion string 
                SqlConnection _miconexion = new SqlConnection();
                _miconexion.ConnectionString = this._cadenaConexionBD;
                _miconexion.Open();

                SqlCommand _buscarPassword = new SqlCommand();
                _buscarPassword.Connection = _miconexion;
                _buscarPassword.CommandType = CommandType.Text;
                //------------------primero deberia recuperar la pass o el email y si los dos campos: ok return true
                _buscarPassword.CommandText = "SELECT HashPasword FROM dbo.Clientes where Email=@email";
                _buscarPassword.Parameters.AddWithValue("@email", _emailcliente);
                String _HaspassDevuelta = (String)_buscarPassword.ExecuteScalar();
                if (_HaspassDevuelta!=null)
                {
                    //comparo la pass con la hashpass q me devolvió
                    Boolean _passwordCorrecta = BCrypt.Net.BCrypt.Verify(_password, _HaspassDevuelta);
                    if (_passwordCorrecta) 
                    {
                        SqlCommand _buscarCLiente = new SqlCommand();
                        _buscarCLiente.Connection = _miconexion;
                        _buscarCLiente.CommandType = CommandType.Text;
                        _buscarCLiente.CommandText = "SELECT Nombre, PrimerApellido FROM dbo.Clientes WHERE Email=@Email";
                        _buscarCLiente.Parameters.AddWithValue("@Email", _emailcliente);
                        String credencialesOK = (String)_buscarCLiente.ExecuteScalar();
                        //si no da error devolvera al controlador credencialesOK==1
                        if (credencialesOK !=null)
                        {
                            return true;
                        }
                        /*  else if(credencialesOK==0)
                          {
                              return false;
                          }*/
                    }
                }

            } 
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return false; //no existe
            }


            return false;

        }


        //------------------------------------------------------------------------------------------------------
        public Cliente DevolverCliente(string emailCliente)
        {
            throw new NotImplementedException();
        }


        //-------------------------------------------------------------------------------------------------------

        public int RegistrarCliente(Cliente newCliente)
        {
            const int workFactor = 13;
            //aqui va el codigo q hace el insert en la tabla Cliente de la BBDD 
            try 
            {
                //se abre conexion a la bbdd con la conexion string 
                SqlConnection _miconexion = new SqlConnection();
                _miconexion.ConnectionString = this._cadenaConexionBD;
                _miconexion.Open();

                SqlCommand _insertCliente = new SqlCommand();
                _insertCliente.Connection = _miconexion;
                _insertCliente.CommandType = CommandType.Text; //<---tipo de comando a lanzar, procedimiento almacernado, instrucicon sql, ...
                _insertCliente.CommandText = "INSERT INTO dbo.Clientes VALUES (@Email,@HashPassword,@Nombre,@PrimerApellido,@SegundoApellido,@NIF,@TelefonoMovil,@TelefonoFijo,@CuentaACtiva)";
                _insertCliente.Parameters.AddWithValue("@Email", newCliente.CredencialesAcceso.Email);
                _insertCliente.Parameters.AddWithValue("@HashPassword", BCrypt.Net.BCrypt.HashPassword(newCliente.CredencialesAcceso.Password, workFactor));
                _insertCliente.Parameters.AddWithValue("@Nombre", newCliente.Nombre);
                _insertCliente.Parameters.AddWithValue("@PrimerApellido", newCliente.PrimerApellido);
                _insertCliente.Parameters.AddWithValue("@SegundoApellido", newCliente.SegundoApellido);
                _insertCliente.Parameters.AddWithValue("@NIF", newCliente.NIF);
                _insertCliente.Parameters.AddWithValue("@TelefonoMovil", newCliente.TelefonoMovil);
                _insertCliente.Parameters.AddWithValue("@TelefonoFijo", newCliente.Telefonofijo);
                _insertCliente.Parameters.AddWithValue("@CuentaActiva", false);

                int _resultado = _insertCliente.ExecuteNonQuery();
               
                    return _resultado;

            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
           
        }

        public Boolean ActivarCuenta(String emailCliente)
        {
            try
            {
                //con el email pasado por la url busco en la BD y cambio el campo CuentaActiva=true
                SqlConnection _miconexion = new SqlConnection();
                _miconexion.ConnectionString = this._cadenaConexionBD;
                _miconexion.Open();

                SqlCommand _buscarCliente = new SqlCommand();
                _buscarCliente.Connection = _miconexion;
                _buscarCliente.CommandType = CommandType.Text;
                _buscarCliente.CommandText = "UPDATE dbo.Clientes SET CuentaActiva=@activa WHERE Email=@email";
                _buscarCliente.Parameters.AddWithValue("@email", emailCliente);
                _buscarCliente.Parameters.AddWithValue("@activa", true);
                int _activacionOK = _buscarCliente.ExecuteNonQuery();
                if (_activacionOK == 1)
                {
                    return true;
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            

            return false;
        }


        #endregion


        #region "...Acceso a las tablas relcionadas con la TIENDA 

        #endregion

    }
}
