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

        Cliente DevolverCliente(String emailCliente);//<--pasa el email y retorna el cleinte 

        Boolean ComprobarCredenciales(String emailcliente, String password);//<---comprobar credenciales para el login del cliente

        Boolean ActivarCuenta(String emailCliente); 


        #endregion

        #region "...Metodos relacionados con el acceso a la BBDD y las tabas relacionadas con la TIENDA Y PRODUCTOS"


        #endregion
    }
}
