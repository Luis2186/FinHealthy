using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.Usuarios.UsuariosDTO
{
    public class UsuarioRolDTO
    {
        [Required]
        public string? idUsuario { get; set; } = string.Empty;
        public string? IdRol { get; set; } = string.Empty;
        public string? NombreRol { get; set; } = string.Empty;
    }
}
