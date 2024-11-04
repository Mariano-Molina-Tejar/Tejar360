using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entity
{
    public class  LoginEntity
    {
        [Required(ErrorMessage = "Ingrese un usuario")]
        [DataType(DataType.EmailAddress)]        
        public string Usuario { get; set; }

        [Required(ErrorMessage = "Ingrese una contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Mensaje { get; set; }
    }
}