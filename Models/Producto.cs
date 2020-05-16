using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HipercoreASPNETCORE.Models
{
    public class Producto
    {
        public String idProducto  { get; set; }
        public String CategoriaProducto  { get; set; }
        public String NombreProducto  { get; set; }
        public Decimal PrecioProducto  { get; set; }
        public String  DescripcionProducto { get; set; }
        public String  Imagen { get; set; }
}
}
