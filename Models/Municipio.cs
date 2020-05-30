using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HipercoreASPNETCORE.Models
{
    public class Municipio
    {
        [Required]
        [RegularExpression("^[0-9]{5}$", ErrorMessage = "Formato invalido p.e: 28805")]
        public String  CodPo { get; set; }
        [RegularExpression("^[0-9]{5}$", ErrorMessage = "Formato invalido p.e: 28079")]
        public String CodMuni  { get; set; }
        [MaxLength(length: 50, ErrorMessage = "Logitud maxima de 50 caracteres")]
        public String  NomMunicipio { get; set; }
}
}
