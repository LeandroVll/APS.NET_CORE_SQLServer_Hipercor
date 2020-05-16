using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;    //<---Validadorres para cada propiedad


namespace HipercoreASPNETCORE.Models
{
    public class Cliente
    {



        [Required(ErrorMessage = "Nombre obligatorio")] //<---creas una instancia de la clase RequiredAttribute y le pasas como argumentos
        [MaxLength(length: 20, ErrorMessage = "Logitud maxima de 20 caracteres")]//al constructor el ErrorMessage...
        public String Nombre { get; set; }

        //----------------
        [Required(ErrorMessage ="El primer apellido es obligatorio")]
        [MaxLength(length:100, ErrorMessage ="Logitud maxima de 100 caracteres")]
        public String PrimerApellido { get; set; }

        //----------------
        [Required(ErrorMessage = "El segundo apellido es obligatorio")]
        [MaxLength(length: 20, ErrorMessage = "Logitud maxima de 20 caracteres")]
        public String SegundoApellido { get; set; }

        //----------------no abligatorio
        public DateTime FechaNacimiento { get; set; }

        //----------------
        public String Telefonofijo{ get; set; }

        //----------------
        [Required(ErrorMessage = "El telefono movil es obligatorio")]
        [RegularExpression("^[0-9]{9}$",ErrorMessage ="Formato de telefono invalido p.e: 612345678")]
        public String TelefonoMovil { get; set; }

        //----------------
        [Required(ErrorMessage = "El telefono movil es obligatorio")]
        [RegularExpression("^[0-9]{8}-[a-zA-Z]$", ErrorMessage = "Formato del DNI es invalido p.e: 12345678-A")]
        public String NIF { get; set; }


        public Credenciales CredencialesAcceso { get; set; }
        public List<Direccion> Direcciones { get; set; }
        

        #region

        //...constructor....
        public Cliente()
        {
            //cada vez q generemos un objeto de tipo cliente, creamos unas credenciales vacias para el...
            this.CredencialesAcceso = new Credenciales();
        }

        #endregion

    }
}
