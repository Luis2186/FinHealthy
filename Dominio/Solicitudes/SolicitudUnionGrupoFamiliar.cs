﻿using Dominio.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Solicitudes
{
    public class SolicitudUnionGrupoFamiliar : Solicitud
    {
        public string UsuarioAdministradorGrupoId { get; set; }
        public Usuario? UsuarioAdministradorGrupo { get; set; }
        public string? CodigoDeSeguridad { get; set; } //Si tiene el codigo de seguridad se une automaticamente
        public ISolicitudUnionGrupoFamiliar EstadoSolicitudGrupo { get; set; }

        public SolicitudUnionGrupoFamiliar()
        {
            
        }

        public SolicitudUnionGrupoFamiliar(Usuario usuarioSolicitante, Usuario _usuarioAdministradorGrupo)
             : base(usuarioSolicitante)
        {
            this.UsuarioSolicitante = usuarioSolicitante;
            this.UsuarioAdministradorGrupo = _usuarioAdministradorGrupo;
            this.EstadoSolicitudGrupo = new SUGF_Pendiente();
            this.Estado = "Pendiente";
            this.FechaDeEnvio = DateTime.Now;
            this.Activa = true;
        }

        public void AsignarUsuarioSolcitante(Usuario usuarioSolicitante)
        {
            this.UsuarioSolicitante = usuarioSolicitante;
        }

        public void AsignarUsuarioDestino(Usuario usuarioDestino)
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
