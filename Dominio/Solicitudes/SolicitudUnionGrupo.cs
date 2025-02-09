using Dominio.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Solicitudes
{
    public class SolicitudUnionGrupo : Solicitud
    {
        public string? UsuarioAdministradorGrupoId { get; set; }
        public Usuario? UsuarioAdministradorGrupo { get; set; }
        public string? CodigoDeSeguridad { get; set; } //Si tiene el codigo de seguridad se une automaticamente
        public ISolicitudUnionGrupo? EstadoSolicitudGrupo { get; set; }

        public SolicitudUnionGrupo()
        {
            
        }

        public SolicitudUnionGrupo(Usuario usuarioSolicitante, Usuario _usuarioAdministradorGrupo)
             : base(usuarioSolicitante)
        {
            this.UsuarioSolicitante = usuarioSolicitante;
            this.UsuarioAdministradorGrupo = _usuarioAdministradorGrupo;
            this.EstadoSolicitudGrupo = new SUG_Pendiente();
            this.Estado = "Pendiente";
            this.FechaDeEnvio = DateTime.Now;
            this.Activa = true;
        }


        public void EnviarSolicitudParaUnirseAGrupo(Usuario usuarioSolicitante, Usuario usuarioDestino)
        {
            AsignarUsuarioSolcitante(usuarioSolicitante);
            AsignarUsuarioDestino(usuarioDestino);
            this.EstadoSolicitudGrupo = new SUG_Pendiente();
            this.Estado = "Pendiente";
            this.FechaDeEnvio = DateTime.Now;
            this.Activa = true;
        }


        private void AsignarUsuarioSolcitante(Usuario usuarioSolicitante)
        {
            this.UsuarioSolicitante = usuarioSolicitante;
        }

        private void AsignarUsuarioDestino(Usuario usuarioDestino)
        {
            this.UsuarioAdministradorGrupo = usuarioDestino;
        }

        public void CambiarEstado(string estado)
        {
            this.Estado = estado;
        }

        public override Resultado<bool> Aceptar() => EstadoSolicitudGrupo.Aceptar(this);
        public override Resultado<bool> Rechazar() => EstadoSolicitudGrupo.Rechazar(this);

    }
}
