﻿using AutoMapper;
using Dominio;
using Dominio.Abstracciones;
using Dominio.Notificaciones;
using Dominio.Usuarios;
using Repositorio.Repositorios.Notificaciones;
using Repositorio.Repositorios.Usuarios;
using Servicio.Notificaciones.NotificacionesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.Notificaciones
{
    public class ServicioNotificacion : IServicioNotificacion
    {
        private readonly IRepositorioNotificacion _repoNotificacion;
        private readonly IRepositorioUsuario _repoUsuario;
        private readonly IMapper _mapper;

        public ServicioNotificacion(IRepositorioNotificacion _repoNotificacion,
            IRepositorioUsuario repositorioUsuario, IMapper mapper)
        {
            this._repoNotificacion = _repoNotificacion;
            this._repoUsuario=repositorioUsuario;
            this._mapper = mapper;
        }
        public async Task<Resultado<NotificacionDTO>> BuscarNotificacion(int notificacionId)
        {
            var resultado = await _repoNotificacion.BuscarNotificacion(notificacionId);

            if (resultado.TieneErrores) return Resultado<NotificacionDTO>.Failure(resultado.Errores);

            var notificacionDTO = _mapper.Map<NotificacionDTO>(resultado.Valor);

            return notificacionDTO;
        }

        public async Task<Resultado<bool>> EliminarNotificacion(int notificacionId)
        {
            return await _repoNotificacion.EliminarNotificacion(notificacionId);
        }

        public async Task<Resultado<NotificacionDTO>> EnviarNotificacion(NotificacionCreacionDTO notificacionCreacionDTO)
        {

            var resultado_UEmisor = await _repoUsuario.ObtenerPorIdAsync(notificacionCreacionDTO.UsuarioEmisorId);
            var resultado_UReceptor = await _repoUsuario.ObtenerPorIdAsync(notificacionCreacionDTO.UsuarioReceptorId);

            if (resultado_UEmisor.TieneErrores) return Resultado<NotificacionDTO>.Failure(new Error("EnviarNotificacion", "El usuario emisor no fue encontrado"));
            if (resultado_UReceptor.TieneErrores) return Resultado<NotificacionDTO>.Failure(new Error("EnviarNotificacion", "El usuario receptor no fue encontrado"));

            var notificacion = _mapper.Map<Notificacion>(notificacionCreacionDTO);

            notificacion.UsuarioEmisor = resultado_UEmisor.Valor;
            notificacion.UsuarioReceptor = resultado_UReceptor.Valor;

            var resultado = await _repoNotificacion.EnviarNotificacion(notificacion);

            if (resultado.TieneErrores) return Resultado<NotificacionDTO>.Failure(resultado.Errores);

            var notificacionDTO = _mapper.Map<NotificacionDTO>(notificacion);

            return notificacionDTO;
        }

        public async Task<Resultado<bool>> MarcarComoLeida(int notificacionId)
        {
            return await _repoNotificacion.MarcarComoLeida(notificacionId);
        }

        public async Task<Resultado<IEnumerable<NotificacionDTO>>> ObtenerNotificacionesEmitidas(string usuarioEmisorId)
        {
            var resultado = await _repoNotificacion.ObtenerNotificacionesEmitidas(usuarioEmisorId);

            if (resultado.TieneErrores) return Resultado<IEnumerable<NotificacionDTO>>.Failure(resultado.Errores);

            var notificacionesDTOS = _mapper.Map<List<NotificacionDTO>>(resultado.Valor);

            return notificacionesDTOS;
        }

        public async Task<Resultado<IEnumerable<NotificacionDTO>>> ObtenerNotificacionesRecibidas(string usuarioReceptorId)
        {
            var resultado = await _repoNotificacion.ObtenerNotificacionesRecibidas(usuarioReceptorId);

            if (resultado.TieneErrores) return Resultado<IEnumerable<NotificacionDTO>>.Failure(resultado.Errores);

            var notificacionesDTOS = _mapper.Map<List<NotificacionDTO>>(resultado.Valor);

            return notificacionesDTOS;
        }
    }
}
