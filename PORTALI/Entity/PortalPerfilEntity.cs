using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entity
{
    public class PortalPerfilEntity
    {   
        public PortalPerfilEntity() 
        {
        }
        public int UserId { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int SlpCode { get; set; }
        public string SlpName { get; set; }

        [Required(ErrorMessage = "Debe ingresar el password actual")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Ingrees una nueva contraseña")]
        [StringLength(255, ErrorMessage = "Debe tener mayor a 5 caratectes", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{0,}$", ErrorMessage = "La contraseña debe contener números, letras y simbolos")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Debe confirmar la contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        [StringLength(255, ErrorMessage = "Debe tener mayor a 5 caratectes", MinimumLength = 5)]
        [DataType(DataType.Password)]        
        public string ConfirmPassword { get; set; }

        public string NombrePic { get; set; }        
    }
}
