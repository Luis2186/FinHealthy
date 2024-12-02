using Dominio.Solicitudes;
using Dominio.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.DTOS.SolicitudesDTO
{
    public class EnviarSolicitudDTO
    {
        [Required (ErrorMessage ="El usuario solicitante es requerido")]
        public string UsuarioSolicitanteId { get; set; }
        [Required(ErrorMessage = "El usuario administrador es requerido")]
        public string UsuarioAdministradorGrupoId { get; set; }
        public DateTime FechaDeEnvio { get; set; } = DateTime.Now;
        public string? MensajeOpcional { get; set; }
        public string? CodigoDeSeguridad { get; set; } //Si tiene el codigo de seguridad se une automaticamente
    }
}
