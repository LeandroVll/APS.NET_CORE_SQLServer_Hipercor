using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HipercoreASPNETCORE.Models
{
    public class Direccion
    {

        public String TipoVia { get; set; }
        public String NombreVia  { get; set; }
        public String Edificio  { get; set; }
        public String Escalera  { get; set; }
        public String Piso  { get; set; }
        public String  Puerta { get; set; }
        //-----imports de las otras clases modelos
        public Provincia Provincia { get; set; }
        public Municipio Localidad { get; set; }  //es el modelo municipio con nombre localidad
    }
}
