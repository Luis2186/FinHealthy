using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.Usuarios.UsuariosDTO
{
    public class UsuarioLoginDTO
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "La contraseña esta vacia.")]
        public string? Password { get; set; }
    }
}
