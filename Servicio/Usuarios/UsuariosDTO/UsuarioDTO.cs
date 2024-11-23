using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.Usuarios.UsuariosDTO
{
    public class UsuarioDTO
    {
        public string Id { get; set; }
        public string? NombreDeUsuario { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public int Edad { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public DateTime FechaDeNacimiento { get; set; }
        public DateTime FechaDeRegistro { get; set; }
        public List<string> Roles { get; set; }
        public bool Activo { get; set; }
    }
}
