using Dominio.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Solicitudes
{
    public abstract class Solicitud
    {
        public int Id { get; set; }
        public string? UsuarioSolicitanteId { get; set; }
        public Usuario? UsuarioSolicitante { get; set; }
        public string? Estado { get; set; } // Ej.: Pendiente, Aprobada, Rechazada
        public DateTime FechaDeEnvio { get; set; }
        public DateTime? FechaDeRespuesta { get; set; }
        public string? MensajeOpcional { get; set; }
        public bool Activa { get; set; }

        protected Solicitud()
        {
            
        }
        protected Solicitud(Usuario usuarioSolicitante)
        {
            UsuarioSolicitante = usuarioSolicitante;
            Estado = "Pendiente";
            FechaDeEnvio = DateTime.Now;
            Activa = true;
        }

        public abstract Resultado<bool> Aceptar();
        public abstract Resultado<bool> Rechazar();
    }
}
