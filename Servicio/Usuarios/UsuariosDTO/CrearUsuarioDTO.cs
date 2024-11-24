using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.Usuarios.UsuariosDTO
{
    public class CrearUsuarioDTO : ActualizarUsuarioDTO
    {
        public DateTime FechaDeRegistro { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, ErrorMessage = "La contraseña debe tener al menos {2} caracteres.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required(ErrorMessage = "La confirmación de la contraseña es obligatoria.")]
        [Compare("Password", ErrorMessage = "La contraseña y su confirmación no coinciden.")]
        [DataType(DataType.Password)]
        public string? ConfirmacionPassword { get; set; }
        public string? Rol { get; set; }

        public CrearUsuarioDTO()
        {
            FechaDeRegistro = DateTime.Now;
        }
    }
}
