using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HipercoreASPNETCORE.Models
{
    public class Direccion
    {
        [Required]
        [MaxLength(length: 50, ErrorMessage = "Logitud maxima de 50 caracteres")]
        public String TipoVia { get; set; }
        [Required]
        [MaxLength(length: 50, ErrorMessage = "Logitud maxima de 50 caracteres")]
        public String NombreVia  { get; set; }
        [Required]
        [RegularExpression("^[0-9]{1,3}$", ErrorMessage = "Formato invalido p.e: 999")]
        public String Edificio  { get; set; }
        
        [MaxLength(length: 50, ErrorMessage = "Logitud maxima de 50 caracteres")]
        public String Escalera  { get; set; }
        [Required]
        [RegularExpression("^[0-9]{0,2}$", ErrorMessage = "Formato invalido p.e: 10")]
        public String Piso  { get; set; }
        [Required]
        [MaxLength(length: 1, ErrorMessage = "Logitud maxima de 1 caracter p.e: G")]
        public String  Puerta { get; set; }
        //-----imports de las otras clases modelos
        public Provincia Provincia { get; set; }
        public Municipio Localidad { get; set; }  //es el modelo municipio con nombre localidad
    }
}
