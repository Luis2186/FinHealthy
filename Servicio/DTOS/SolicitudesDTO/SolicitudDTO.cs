using Dominio.Solicitudes;
using Dominio.Usuarios;
using Servicio.DTOS.UsuariosDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.DTOS.SolicitudesDTO
{
    public class SolicitudDTO
    {
        public int Id { get; set; }
        public string UsuarioSolicitanteId { get; set; }
        public string UsuarioAdministradorGrupoId { get; set; }
        public string? MensajeOpcional { get; set; }
        public string? Estado { get; set; } // Ej.: Pendiente, Aprobada, Rechazada
        public DateTime FechaDeEnvio { get; set; }
        public DateTime? FechaDeRespuesta { get; set; }
        public bool Activa { get; set; }
        public UsuarioDTO? UsuarioSolicitante { get; set; }
        public UsuarioDTO? UsuarioAdministradorGrupo { get; set; }
    }
}
