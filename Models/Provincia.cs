using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HipercoreASPNETCORE.Models
{
    public class Provincia
    {
        [MaxLength(length: 02, ErrorMessage = "Logitud maxima de 02 caracteres")]
        public String CodProv  { get; set; }
        [Required]
        [MaxLength(length: 50, ErrorMessage = "Logitud maxima de 50 caracteres")]
        public String  NomProvincia { get; set; }
}
}
