﻿using Dominio;
using Servicio.DTOS.GruposDTO;
using Servicio.DTOS.SolicitudesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.S_Grupos
{
    public interface IServicioGrupos
    {
        public Task<Resultado<GrupoDTO>> ObtenerGrupoPorId(int id);
        public Task<Resultado<IEnumerable<GrupoDTO>>> ObtenerTodosLosGrupos();
        public Task<Resultado<IEnumerable<GrupoDTO>>> ObtenerTodosLosGruposPorUsuario(string idUsuario);
        public Task<Resultado<GrupoDTO>> CrearGrupo(CrearGrupoDTO grupoCreacionDTO);
        public Task<Resultado<GrupoDTO>> ActualizarGrupo(int grupoId,ActualizarGrupoDTO grupoActualizacionDTO);
        public Task<Resultado<bool>> EliminarGrupo(int id);
        public Task<Resultado<bool>> AceptarSolicitudIngresoAGrupo(int idSolicitud);
        public Task<Resultado<bool>> RechazarSolicitudIngresoAGrupo(int idSolicitud);
        public Task<Resultado<SolicitudDTO>> EnviarSolicitudIngresoAGrupo(EnviarSolicitudDTO solicitud);
        public Task<Resultado<IEnumerable<SolicitudDTO>>> ObtenerSolicitudesPorAdministrador(string idAdministrador, string estado);
        public Task<Resultado<bool>> IngresoAGrupoConCodigo(UnirseAGrupoDTO acceso);
    }
}
