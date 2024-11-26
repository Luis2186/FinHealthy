using Dominio.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Solicitudes
{
    public class SolicitudUnionGrupoFamiliar 
    {
        public int Id { get; set; }
        public Usuario? UsuarioSolicitante { get; set; }
        public Usuario? usuarioAdministradorGrupo { get; set; }
        public string? CodigoDeSeguridad { get; set; } //Si tiene el codigo de seguridad se une automaticamente
        public string? Estado { get; set; }
        public DateTime FechaDeEnvio { get; set; }
        public DateTime FechaDeRespuesta { get; set; }
        public string? MensajeOpcional { get; set; }
        public ISolicitudUnionGrupoFamiliar EstadoSolicitudGrupo { get; set; }

        public SolicitudUnionGrupoFamiliar(Usuario usuarioSolicitante, Usuario _usuarioAdministradorGrupo)
        {
            this.UsuarioSolicitante = usuarioSolicitante;
            this.usuarioAdministradorGrupo = _usuarioAdministradorGrupo;
            this.EstadoSolicitudGrupo = new SUGF_Pendiente();
            this.Estado = "Pendiente";
            this.FechaDeEnvio = DateTime.Now;
        }

        public void AsignarUsuarioSolcitante(Usuario usuarioSolicitante)
        {
            this.UsuarioSolicitante = usuarioSolicitante;
        }

        public void AsignarUsuarioDestino(Usuario usuarioDestino)
        {
            this.usuarioAdministradorGrupo = usuarioDestino;
        }

        public void CambiarEstado(string estado)
        {
            this.Estado = estado;
        }

        public Resultado<bool> Aceptar() => EstadoSolicitudGrupo.Aceptar(this);
        public Resultado<bool> Rechazar() => EstadoSolicitudGrupo.Rechazar(this);

    }
}
