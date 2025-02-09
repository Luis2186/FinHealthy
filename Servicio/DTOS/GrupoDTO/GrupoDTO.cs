using Dominio.Usuarios;
using Servicio.DTOS.UsuariosDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.DTOS.GrupoDTO
{
    public class GrupoDTO
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaDeCreacion { get; set; }
        public string UsuarioAdministradorId { get; set; }
        public List<UsuarioDTO> Miembros { get; set; }
        public bool Activo { get; set; }
    }
}
