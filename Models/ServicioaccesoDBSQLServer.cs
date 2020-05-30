using HipercoreASPNETCORE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//Espacio de nombres relacionados, necesarios para hacer operaciones coontra sqlServer
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

namespace HipercoreASPNETCORE.Models
{

   
    public class ServicioaccesoDBSQLServer : IServicioAccesoBD
    {


         String _cadenaConexionBD=@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=HipercorDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


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
                //----primero deberia recuperar la pass o el email y si los dos campos: ok return true
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
        public Cliente DevolverCliente(Cliente cliente)
        {
            //Recuperar datos del cliente de la tabla cliente (Nombre,Primerapellido, NIF...)
            //devolverlo en un obj cliente
            
            //accedo a la info de la tabla y la cargo en un obj reader
            try
            {
                SqlConnection _miconexion = new SqlConnection();
                _miconexion.ConnectionString = this._cadenaConexionBD;
                _miconexion.Open();

                SqlCommand _buscarCliente = new SqlCommand();
                _buscarCliente.Connection = _miconexion;
                _buscarCliente.CommandType = CommandType.Text;
                _buscarCliente.CommandText = "SELECT * FROM dbo.Clientes WHERE Email=@email";
                _buscarCliente.Parameters.AddWithValue("@email", cliente.CredencialesAcceso.Email);
                SqlDataReader _reader = _buscarCliente.ExecuteReader(); //<---devuelve la fila entera en el reader

                //recorro el reader y gurado en el obj cliente los datos
                if (_reader.HasRows) 
                {
                    while (_reader.Read())
                    {
                        cliente.Nombre = _reader.GetString(2); //<--posicion en la tabla
                        cliente.PrimerApellido = _reader.GetString(3);
                        cliente.SegundoApellido = _reader.GetString(4);
                        cliente.NIF = _reader.GetString(5);
                        cliente.TelefonoMovil = _reader.GetString(6);
                        cliente.Telefonofijo = _reader.GetString(7);
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return cliente; //<---devuelvo el obj cliente con los datos de la tabla Cliente

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

        public Boolean ComprobarCuentaActiva(String emailCliente)
        {


            try
            {
                //con el email pasado por la url busco en la BD y extraigo el campo CuentaActiva
                SqlConnection _miconexion = new SqlConnection();
                _miconexion.ConnectionString = this._cadenaConexionBD;
                _miconexion.Open();

                SqlCommand _buscarCliente = new SqlCommand();
                _buscarCliente.Connection = _miconexion;
                _buscarCliente.CommandType = CommandType.Text;
                _buscarCliente.CommandText = "SELECT CuentaActiva FROM dbo.Clientes WHERE Email=@Email";
                _buscarCliente.Parameters.AddWithValue("@email", emailCliente);
                Boolean _activaOK = (Boolean)_buscarCliente.ExecuteScalar();  
                if (_activaOK)                                              //<--si CuentaActiva=true devuelve true al controlador
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

        //--------------------------------------------------TIENDA--------------------------------------------------------
        #region "...Acceso a las tablas relcionadas con la TIENDA 

        //---tengo que llamar a la tabla Direcciones y devolver el resulatdo en un array con las direcciones...el modelo cliente 
        //  devuelve un arrayList de Direcciones
        public Cliente CargarListaDireciones(Cliente cliente)
        {
            try
            {
                //busco en la tabla la direccion el cleinte sus credencailes.Email
                //y meto en un array list de la clase Cleinte.Direcciones
                cliente.Direcciones = new List<Direccion>(); //<----instancio un array<Direcciones>

                SqlConnection _miconexion = new SqlConnection();
                _miconexion.ConnectionString = this._cadenaConexionBD;
                _miconexion.Open();

                SqlCommand _buscarCliente = new SqlCommand();
                _buscarCliente.Connection = _miconexion;
                _buscarCliente.CommandType = CommandType.Text;
                _buscarCliente.CommandText = "SELECT * FROM dbo.Direccion WHERE Email=@email";
                _buscarCliente.Parameters.AddWithValue("@email", cliente.CredencialesAcceso.Email);
                SqlDataReader _reader = _buscarCliente.ExecuteReader(); //<---devuelve la fila entera en el reader

                //recorro el reader y guardo los valores de las columnas en un obj de tipo direcciones
                // y este es el q se gurda en el array del cliente en concreto q se le pasa ...
                //declaro 1 objeto de clada clase q contienen la clase Direccion.cs y gurado en cada obj el valor de cada columna 
                Direccion direccion = new Direccion();
                direccion.Provincia = new Provincia();
                direccion.Localidad = new Municipio();

                if (_reader.HasRows) //<---si hay direcciones ??
                {

                    while ( _reader.Read())
                    {

                        //no puedo guradar directamente los valores en el array<Direcciones>
                        direccion.Localidad.CodPo = _reader.GetString(1); //<--hay q indicarle la posicion de la columna en DB
                        direccion.Localidad.CodMuni = _reader.GetString(2);
                        direccion.Localidad.NomMunicipio = _reader.GetString(3);
                        direccion.Provincia.CodProv = _reader.GetString(4);
                        direccion.Provincia.NomProvincia = _reader.GetString(5);
                        direccion.TipoVia = _reader.GetString(6);
                        direccion.NombreVia = _reader.GetString(7);
                        direccion.Edificio = _reader.GetString(8);
                        direccion.Escalera = _reader.GetString(9);
                        direccion.Piso = _reader.GetString(10);
                        direccion.Puerta = _reader.GetString(11);

                        cliente.Direcciones.Add(direccion);//<--gurado el obj Direciones
                    }
                }
                else
                {
                    Console.WriteLine("...No tiene direcciones...");
                }
                _reader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            
            return cliente; //<--- devuelve el obj cliente con un array de Direcciones, si tiene..
        }

        //------------------------------------------------------------------------------------------------------------------------------
        public int RegistrarDireccion(Cliente cliente) 
        {

            try
            {
                int _resultado = 0;
                //inserto lo parametros q contiene el obj array de direcciones del obj cliente
                SqlConnection _miconexion = new SqlConnection();
                _miconexion.ConnectionString = this._cadenaConexionBD;
                _miconexion.Open();

                SqlCommand _insertarDireccion = new SqlCommand();
                _insertarDireccion.Connection = _miconexion;
                _insertarDireccion.CommandType = CommandType.Text;
                
                foreach (Direccion item in cliente.Direcciones) //<---pasar a aar primero
                {
                    _insertarDireccion.CommandText = "INSERT INTO dbo.Direccion VALUES (@email, @codPo, @codMuni, @nomMunicipio, @codProv, " +
                                                      "@nomProvincia, @tipoVia, @nombreVia, @edificio, @escalera, @piso, @puerta)";
                    _insertarDireccion.Parameters.AddWithValue("@email", cliente.CredencialesAcceso.Email);
                    _insertarDireccion.Parameters.AddWithValue("@codPo", item.Localidad.CodPo); 
                    _insertarDireccion.Parameters.AddWithValue("@codMuni", item.Localidad.CodMuni);
                    _insertarDireccion.Parameters.AddWithValue("@nomMunicipio", item.Localidad.NomMunicipio);
                    _insertarDireccion.Parameters.AddWithValue("@codProv", item.Provincia.CodProv);
                    _insertarDireccion.Parameters.AddWithValue("@nomProvincia", item.Provincia.NomProvincia);
                    _insertarDireccion.Parameters.AddWithValue("@tipoVia", item.TipoVia);
                    _insertarDireccion.Parameters.AddWithValue("@nombreVia", item.NombreVia);
                    _insertarDireccion.Parameters.AddWithValue("@edificio", item.Edificio);
                    _insertarDireccion.Parameters.AddWithValue("@escalera", item.Escalera);
                    _insertarDireccion.Parameters.AddWithValue("@piso", item.Piso);
                    _insertarDireccion.Parameters.AddWithValue("@puerta", item.Puerta);
                    _resultado=_insertarDireccion.ExecuteNonQuery(); //hay un error con el formato de los datos y la tabla
                    break;
                }

               
                if (_resultado == 1)
                {
                    return _resultado;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
            return 0;
        }

        //--------------------------------productos de escaparate de la tienda-------------------------------------
                
        public List<Producto> TodosLosProductos()
        {

            List<Producto> allProducts = new List<Producto>();
            

            try
            {
                
                SqlConnection _miconexion = new SqlConnection();
                _miconexion.ConnectionString = this._cadenaConexionBD;
                _miconexion.Open();

                SqlCommand _buscarCliente = new SqlCommand();
                _buscarCliente.Connection = _miconexion;
                _buscarCliente.CommandType = CommandType.Text;
                _buscarCliente.CommandText = "SELECT * FROM dbo.Producto";
                SqlDataReader _reader = _buscarCliente.ExecuteReader();
                               

                if (_reader.HasRows) 
                {

                    while (_reader.Read())
                    {
                        Producto unProducto = new Producto();
                        unProducto.idProducto = _reader.GetString(0);
                        unProducto.CategoriaProducto = _reader.GetString(1);
                        unProducto.NombreProducto = _reader.GetString(2);
                        unProducto.PrecioProducto = _reader.GetDecimal(3);
                        unProducto.DescripcionProducto = _reader.GetString(4);
                        unProducto.Imagen = _reader.GetString(5);

                        allProducts.Add(unProducto);

                    }

                    return allProducts;
                }
                else
                {
                    Console.WriteLine("...No hay productos en la BD...");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return allProducts;
        }
        //---------------------------------------Inserta productos---------------------------------------------------





        //-------------------------------------buscar por id y devolver el profucto ----------------------------------
        public Producto DevolverProducto(int id)
        {
            Producto _producto = new Producto();
            try
            {

                SqlConnection _miconexion = new SqlConnection();
                _miconexion.ConnectionString = this._cadenaConexionBD;
                _miconexion.Open();

                SqlCommand _buscarProducto = new SqlCommand();
                _buscarProducto.Connection = _miconexion;
                _buscarProducto.CommandType = CommandType.Text;
                _buscarProducto.CommandText = "SELECT * FROM dbo.Producto WHERE idProducto=@id";
                _buscarProducto.Parameters.AddWithValue("@id", id);
                SqlDataReader _reader = _buscarProducto.ExecuteReader();

                if (_reader.HasRows) 
                {
                    while (_reader.Read())
                    {
                        _producto.idProducto = _reader.GetString(0);
                        _producto.CategoriaProducto = _reader.GetString(1);
                        _producto.NombreProducto = _reader.GetString(2);
                        _producto.PrecioProducto = _reader.GetDecimal(3);
                        _producto.DescripcionProducto = _reader.GetString(4);
                        _producto.Imagen = _reader.GetString(5);
                    }

                    return _producto;
                }

            }
            catch (Exception ex)
            {
                
                throw;

            }


            return _producto;
        }


        //-----------------------insertar pedido----------------------------
        public Boolean InsertarPedido(Pedido pedido)
        {
            try
            {

                SqlConnection _miconexion = new SqlConnection();
                _miconexion.ConnectionString = this._cadenaConexionBD;
                _miconexion.Open();

                SqlCommand _insertarPedido = new SqlCommand();
                _insertarPedido.Connection = _miconexion;
                _insertarPedido.CommandType = CommandType.Text;
                _insertarPedido.CommandText = "INSERT INTO dbo.Pedido values (@idPedido, @NifCliente, @FechaPedido," +
                                                " @EstadoPedido, @ListaElementosPedidos, @TipoGastosEnvio, @GastosEnvio," +
                                                " @SubTotalPedido, @TotalPedido)";
                _insertarPedido.Parameters.AddWithValue("@idPedido", pedido.idPedido);
                _insertarPedido.Parameters.AddWithValue("@NifCliente", pedido.NifCliente);
                _insertarPedido.Parameters.AddWithValue("@FechaPedido", pedido.FechaPedido);
                _insertarPedido.Parameters.AddWithValue("@EstadoPedido", pedido.EstadoPedido);
                 var listaSerializada = JsonConvert.SerializeObject(pedido.ListaElementosPedidos);
                // listaSerializada.Replace(@"\", string.Empty);
                _insertarPedido.Parameters.AddWithValue("@ListaElementosPedidos", listaSerializada);
                _insertarPedido.Parameters.AddWithValue("@TipoGastosEnvio", pedido.TipoGastosEnvio);
                _insertarPedido.Parameters.AddWithValue("@GastosEnvio", pedido.GastosEnvio);
                _insertarPedido.Parameters.AddWithValue("@SubTotalPedido", pedido.SubTotalPedido);
                _insertarPedido.Parameters.AddWithValue("@TotalPedido", pedido.TotalPedido);
                int _filas=_insertarPedido.ExecuteNonQuery();
                if (_filas==1)
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

    //-----------------------devuelve los pedidos asociados un NIF-----------------------------
        public List<Pedido> DevolverPedisosList(String nif)
        {
            List<Pedido> listaPedidos = new List<Pedido>();
            try
            {
                SqlConnection _miconexion = new SqlConnection();
                _miconexion.ConnectionString = this._cadenaConexionBD;
                _miconexion.Open();

                SqlCommand _buscarPedidos = new SqlCommand();
                _buscarPedidos.Connection = _miconexion;
                _buscarPedidos.CommandType = CommandType.Text;
                _buscarPedidos.CommandText = "SELECT * FROM dbo.Pedido WHERE NifCliente=@nif";
                _buscarPedidos.Parameters.AddWithValue("@nif", nif);
                SqlDataReader _reader = _buscarPedidos.ExecuteReader();

                if (_reader.HasRows) 
                {
                    while (_reader.Read())
                    {

                        Pedido unPedido = new Pedido();
                        unPedido.idPedido = _reader.GetString(0);
                        unPedido.NifCliente = _reader.GetString(1);
                        unPedido.FechaPedido = _reader.GetDateTime(2);
                        unPedido.EstadoPedido = _reader.GetString(3);
                        var listaserializada= _reader.GetString(4);
                        unPedido.ListaElementosPedidos = JsonConvert.DeserializeObject<Dictionary<string, int>>(listaserializada);
                        unPedido.TipoGastosEnvio = _reader.GetString(5);
                        unPedido.GastosEnvio = _reader.GetDecimal(6);
                        unPedido.SubTotalPedido = _reader.GetDecimal(7);
                        unPedido.TotalPedido = _reader.GetDecimal(8);

                        listaPedidos.Add(unPedido);

                    }
                    return listaPedidos;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        
            return listaPedidos;
        }


        //----------------------------------------------actualizar pedido--------------------------------------------

        public Boolean ActualizarPedido(Pedido pedido)
        {
            try
            {
                //busco el pedido
                SqlConnection _miconexion = new SqlConnection();
                _miconexion.ConnectionString = this._cadenaConexionBD;
                _miconexion.Open();

                SqlCommand _buscarPedido = new SqlCommand();
                _buscarPedido.Connection = _miconexion;
                _buscarPedido.CommandType = CommandType.Text;
                _buscarPedido.CommandText = "UPDATE dbo.Pedido SET SubTotalPedido=@subTotalPedido, TotalPedido=@totalPedido WHERE idPedido=@idPedid";
                _buscarPedido.Parameters.AddWithValue("@subTotalPedido", pedido.SubTotalPedido);
                _buscarPedido.Parameters.AddWithValue("@totalPedido", pedido.TotalPedido);
                _buscarPedido.Parameters.AddWithValue("@idPedid", pedido.idPedido);
                int _activacionOK = _buscarPedido.ExecuteNonQuery();
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

        //-----------------------------------actualizar estado del pedido ------------------------------------------
        public Boolean EstadodelEnvio(String id, string estado)
        {
            try
            {
                //busco el pedido
                SqlConnection _miconexion = new SqlConnection();
                _miconexion.ConnectionString = this._cadenaConexionBD;
                _miconexion.Open();

                SqlCommand _buscarPedido = new SqlCommand();
                _buscarPedido.Connection = _miconexion;
                _buscarPedido.CommandType = CommandType.Text;
                _buscarPedido.CommandText = "UPDATE dbo.Pedido SET EstadoPedido=@estadoPedido WHERE idPedido=@idPedid";
                _buscarPedido.Parameters.AddWithValue("@estadoPedido", estado);
                _buscarPedido.Parameters.AddWithValue("@idPedid", id);
                int _estadoActualizado = _buscarPedido.ExecuteNonQuery();
                if (_estadoActualizado == 1)
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


        //-----------------------------------devolver un pedido ------------------------------------------
        public Pedido DevolverUnPedido(String id)
        {
            Pedido unPedido = unPedido = new Pedido();
            try
            {
                //busco el pedido
                SqlConnection _miconexion = new SqlConnection();
                _miconexion.ConnectionString = this._cadenaConexionBD;
                _miconexion.Open();

                SqlCommand _buscarPedido = new SqlCommand();
                _buscarPedido.Connection = _miconexion;
                _buscarPedido.CommandType = CommandType.Text;
                _buscarPedido.CommandText = "SELECT * FROM dbo.Pedido WHERE idPedido=@idPedid";
                _buscarPedido.Parameters.AddWithValue("@idPedid", id);
                SqlDataReader _reader = _buscarPedido.ExecuteReader();
                
                if (_reader.HasRows)
                {

                    while (_reader.Read())
                    {
                       
                        unPedido.idPedido = _reader.GetString(0);
                        unPedido.NifCliente = _reader.GetString(1);
                        unPedido.FechaPedido = _reader.GetDateTime(2);
                        unPedido.EstadoPedido = _reader.GetString(3);
                        var listaserializada = _reader.GetString(4);
                        unPedido.ListaElementosPedidos = JsonConvert.DeserializeObject<Dictionary<string, int>>(listaserializada);
                        unPedido.TipoGastosEnvio = _reader.GetString(5);
                        unPedido.GastosEnvio = _reader.GetDecimal(6);
                        unPedido.SubTotalPedido = _reader.GetDecimal(7);
                        unPedido.TotalPedido = _reader.GetDecimal(8);

                        return unPedido;
                    }
                                       
                }
                else
                {
                    Console.WriteLine("...No esta el pedido en la BD...");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return unPedido;
        }



        //-------------------------------
        public Boolean EliminarUnPedido(String id)
        {
            try
            {
                //busco el pedido
                SqlConnection _miconexion = new SqlConnection();
                _miconexion.ConnectionString = this._cadenaConexionBD;
                _miconexion.Open();

                SqlCommand _buscarPedido = new SqlCommand();
                _buscarPedido.Connection = _miconexion;
                _buscarPedido.CommandType = CommandType.Text;
                _buscarPedido.CommandText = "DELETE FROM dbo.Pedido WHERE idPedido=@idPedid";
                _buscarPedido.Parameters.AddWithValue("@idPedid", id);
                int filas= _buscarPedido.ExecuteNonQuery();
                if (filas==1)
                {
                    return true;
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }


        public Boolean ModificarPedidoEntero(Pedido pedido)
        {

            try
            {
                //busco el pedido
                SqlConnection _miconexion = new SqlConnection();
                _miconexion.ConnectionString = this._cadenaConexionBD;
                _miconexion.Open();

                SqlCommand _buscarPedido = new SqlCommand();
                _buscarPedido.Connection = _miconexion;
                _buscarPedido.CommandType = CommandType.Text;
                _buscarPedido.CommandText = "UPDATE dbo.Pedido SET NifCliente=@NifCliente, FechaPedido=@FechaPedido," +
                                  " EstadoPedido=@EstadoPedido, ListaElementosPedidos=@ListaElementosPedidos, TipoGastosEnvio=@TipoGastosEnvio, GastosEnvio=@GastosEnvio," +
                                  " SubTotalPedido=@SubTotalPedido, TotalPedido=@TotalPedido WHERE idPedido=@idPedido";

                _buscarPedido.Parameters.AddWithValue("@NifCliente", pedido.NifCliente);
                _buscarPedido.Parameters.AddWithValue("@FechaPedido", pedido.FechaPedido);
                _buscarPedido.Parameters.AddWithValue("@EstadoPedido", pedido.EstadoPedido);
                var listaSerializada = JsonConvert.SerializeObject(pedido.ListaElementosPedidos);
                // listaSerializada.Replace(@"\", string.Empty);
                _buscarPedido.Parameters.AddWithValue("@ListaElementosPedidos", listaSerializada);
                _buscarPedido.Parameters.AddWithValue("@TipoGastosEnvio", pedido.TipoGastosEnvio);
                _buscarPedido.Parameters.AddWithValue("@GastosEnvio", pedido.GastosEnvio);
                _buscarPedido.Parameters.AddWithValue("@SubTotalPedido", pedido.SubTotalPedido);
                _buscarPedido.Parameters.AddWithValue("@TotalPedido", pedido.TotalPedido);
                _buscarPedido.Parameters.AddWithValue("@idPedido", pedido.idPedido);
                int _filas = _buscarPedido.ExecuteNonQuery();
                if (_filas == 1)
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










    }



}
