using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.Usuarios.UsuariosDTO
{
    public class CrearUsuarioDTO :ActualizarUsuarioDTO
    {
        public DateTime FechaDeRegistro { get; set; }
        public string? Password { get; set; }
        public string? ConfirmacionPassword { get; set; }
        public string? Rol { get; set; }

        public CrearUsuarioDTO()
        {
            FechaDeRegistro = DateTime.Now;
        }
    }
}
