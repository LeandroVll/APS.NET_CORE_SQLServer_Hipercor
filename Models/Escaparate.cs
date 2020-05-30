using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HipercoreASPNETCORE.Models
{
    public class Escaparate
    {

        public List<Producto> ListaDatosProducto { get; set; }
        public List<Pedido> ListDatosPedido { get; set; }
        public Escaparate()

        {

            this.ListDatosPedido = new List<Pedido>();

            this.ListaDatosProducto = new List<Producto>();
        }

    }
}
