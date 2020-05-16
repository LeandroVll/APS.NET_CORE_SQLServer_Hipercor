using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HipercoreASPNETCORE.Models
{
    public class Pedido
    {
        public String idPedido  { get; set; }
        public String  NifCliente { get; set; }
        public DateTime FechaPedido  { get; set; }
        public String  EstadoPedido { get; set; }
        public Dictionary<Producto, int> ListaElementosPedidos   { get; set; } //aqui va el id del producto
        public String TipoGastosEnvio  { get; set; }
        public Decimal GastosEnvio  { get; set; }
        public Decimal SubTotalPedido { get; set; }
        public Decimal TotalPedido  { get; set; }


}
}
