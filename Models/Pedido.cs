using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HipercoreASPNETCORE.Models
{
    public class Pedido
    {
        
        public String idPedido  { get; set; } //<--pk_autoincrement
        
        public String  NifCliente { get; set; }
        
        public DateTime FechaPedido  { get; set; }
        public String  EstadoPedido { get; set; } //<--tiene q poder actualizarse despues 
       
        public Dictionary<String, int> ListaElementosPedidos   { get; set; } //aqui va el id del producto y cantidad 
        public String TipoGastosEnvio  { get; set; }//<--tiene q poder actualizarse despues
        public Decimal GastosEnvio  { get; set; }//<--tiene q poder actualizarse despues
        public Decimal SubTotalPedido { get; set; }//<--tiene q poder actualizarse despues
        public Decimal TotalPedido  { get; set; }//<--tiene q poder actualizarse despues

       
    }
}
