using HipercoreASPNETCORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HipercoreASPNETCORE.Interfaces
{
    public interface IServicioAccesoBD
    {
        #region "...Metodos relacionados con el acceso a la BBDD y las tabas relacionadas con el CLEINTE"
  
        int RegistrarCliente(Cliente newCliente); //<--metodo registrar cleinte la BD devuelve un int
                                                  //...porq devuelve un numero de columnas 

        Cliente DevolverCliente(Cliente cliente);//<--pasa el email y retorna el cleinte 

        Boolean ComprobarCredenciales(String emailcliente, String password);//<---comprobar credenciales para el login del cliente

        Boolean ActivarCuenta(String emailCliente); //<---busca el usuario y actualiza el campo CuentaActiva a true

        bool ComprobarCuentaActiva(string emailCliente); //<---busca el campo CuentaActiva del usuario y comprueba si es true

        Cliente CargarListaDireciones(Cliente cliente);

        List<Producto> TodosLosProductos(); //<--devuleve un array de obj con los productos 

        Producto DevolverProducto(int id);//<--devuelve un producto por Id

        #endregion

        #region "...Metodos relacionados con el acceso a la BBDD y las tabas relacionadas con la TIENDA Y PRODUCTOS"

        int RegistrarDireccion(Cliente cliente);

        Boolean InsertarPedido(Pedido pedido);


        List<Pedido> DevolverPedisosList(String nif);

        Boolean ActualizarPedido(Pedido pedido);

        Pedido DevolverUnPedido(String id);

        Boolean EliminarUnPedido(String id);

        Boolean EstadodelEnvio(String id, string estado);

        Boolean ModificarPedidoEntero(Pedido pedido);

        #endregion
    }
}
