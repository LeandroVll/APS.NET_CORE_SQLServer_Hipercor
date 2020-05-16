using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HipercoreASPNETCORE.Models
{
    public class Credenciales
    {
        [Required(ErrorMessage = "Email obligatorio")]
        [RegularExpression("^.*@.*\\..*$", ErrorMessage = "formato de email invalido: julianR@yahoo.es")]
        public String Email { get; set; }


        [Required(ErrorMessage = "no se admiten passwords en blanco")]
        [MinLength(length: 6, ErrorMessage = "password demasiado corta, minimo 6 caracteres numericos y alfanumericos")]
        [MaxLength(length: 10, ErrorMessage = "password demasiado larga, maximo de 10 caracteres numericos y alfanumericos")]
        [RegularExpression("^[a-zA-Z0-9!$%&/()-_]{6,10}$", ErrorMessage = "caracteres invalidos en la password, solo numeros, letras y simbolos determinados")]
        public String Password { get; set; }


        [Required(ErrorMessage = "no se admiten passwords en blanco")]
        [MinLength(length: 6, ErrorMessage = "password demasiado corta, minimo 6 caracteres numericos y alfanumericos")]
        [MaxLength(length: 10, ErrorMessage = "password demasiado larga, maximo de 10 caracteres numericos y alfanumericos")]
        [RegularExpression("^[a-zA-Z0-9!$%&/()-_]{6,10}$", ErrorMessage = "caracteres invalidos en la password, solo numeros, letras y simbolos determinados")]
        public String RePassword { get; set; }
    }

    //cosntructor 
     public class ClienteExisteAttribute: ValidationAttribute
     {

     }


}
