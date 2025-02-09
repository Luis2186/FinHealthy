using AutoMapper;
using Dominio;
using Dominio.Abstracciones;
using Dominio.Errores;
using Dominio.Grupos;
using Dominio.Solicitudes;
using Repositorio.Repositorios;
using Repositorio.Repositorios.R_Grupo;
using Repositorio.Repositorios.Solicitudes;
using Repositorio.Repositorios.Usuarios;
using Servicio.DTOS.GruposDTO;
using Servicio.DTOS.SolicitudesDTO;

namespace Servicio.S_Grupos
{
    public class ServicioGrupos : IServicioGrupos
    {
        private readonly IRepositorioUsuario _repoUsuarios;
        private readonly IRepositorioGrupo _repoGrupo;
        private readonly IRepositorioSolicitud _repositorioSolicitud;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ServicioGrupos(IRepositorioGrupo repoGrupo,
                               IRepositorioUsuario repositorioUsuario,
                               IRepositorioSolicitud repositorioSolicitud,
                               IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repoGrupo = repoGrupo;
            _repoUsuarios = repositorioUsuario;
            _repositorioSolicitud = repositorioSolicitud;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Resultado<bool>> AceptarSolicitudIngresoAGrupo(int idSolicitud)
        {
            try
            {
                var solicitudBuscada = await _repositorioSolicitud.ObtenerPorIdAsync(idSolicitud);

                if (solicitudBuscada.TieneErrores) return Resultado<bool>.Failure(solicitudBuscada.Errores);

                SolicitudUnionGrupo solicitudParaAceptar = solicitudBuscada.Valor;

                if (solicitudParaAceptar.Estado != "Pendiente") return Resultado<bool>.Failure(ErroresSolicitud.Estado_No_Es_Pendiente);

                var grupoAdmin = await _repoGrupo.ObtenerGrupoPorIdAdministrador(solicitudParaAceptar.UsuarioAdministradorGrupoId);
                
                if(grupoAdmin.TieneErrores) return Resultado<bool>.Failure(grupoAdmin.Errores);

                var solicitudAceptada =await _repositorioSolicitud.AceptarSolicitud(idSolicitud);

                if (solicitudAceptada.TieneErrores) return Resultado<bool>.Failure(solicitudAceptada.Errores);

                var usuarioIngresado =await IngresarAGrupo(grupoAdmin.Valor.Id, solicitudParaAceptar.UsuarioSolicitanteId);

                if(usuarioIngresado.EsCorrecto)
                {              
                    return Resultado<bool>.Success(usuarioIngresado.EsCorrecto);
                }

                return Resultado<bool>.Failure(usuarioIngresado.Errores);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion("Servicio.AceptarSolicitudIngresoAGrupo", ex.Message));
            }
        }

        public async Task<Resultado<GrupoDTO>> ActualizarGrupo(int grupoId,ActualizarGrupoDTO grupoActDTO)
        {
            try
            {
                var grupoBuscado = await _repoGrupo.ObtenerPorIdAsync(grupoId);

                if (grupoBuscado.TieneErrores) return Resultado<GrupoDTO>.Failure(grupoBuscado.Errores);

                _mapper.Map(grupoActDTO, grupoBuscado.Valor); // Actualizar el usuario con el DTO

                var resultado = await _repoGrupo.ActualizarAsync(grupoBuscado.Valor);

                if (resultado.TieneErrores) return Resultado<GrupoDTO>.Failure(resultado.Errores);

                var grupoDTO = _mapper.Map<GrupoDTO>(resultado.Valor);

                return Resultado<GrupoDTO>.Success(grupoDTO);
            }
            catch (Exception ex)
            {
                return Resultado<GrupoDTO>.Failure(ErroresCrud.ErrorDeExcepcion("Servicio.ActualizarGrupo", ex.Message));
            }
        }

        public async Task<Resultado<GrupoDTO>> CrearGrupo(CrearGrupoDTO grupoCreacionDTO)
        {
            try
            {
                await _unitOfWork.IniciarTransaccionAsync();

                var usuarioAdmin =await _repoUsuarios.ObtenerPorIdAsync(grupoCreacionDTO.UsuarioAdministradorId);

                if (usuarioAdmin.TieneErrores || usuarioAdmin.Valor == null) return Resultado<GrupoDTO>.Failure(usuarioAdmin.Errores);

                var usuarioAdm = usuarioAdmin.Valor;

                var grupo = _mapper.Map<Grupo>(grupoCreacionDTO);

                usuarioAdm.UnirseAGrupo(grupo);

                grupo.UsuarioAdministrador = usuarioAdm;
                grupo.EstablecerCodigo(grupoCreacionDTO.CodigoAcceso);

                var resultado = await _repoGrupo.CrearAsync(grupo);

                await _repoUsuarios.ActualizarAsync(usuarioAdm);

                if (resultado.TieneErrores) return Resultado<GrupoDTO>.Failure(resultado.Errores);

                var grupoDTO = _mapper.Map<GrupoDTO>(resultado.Valor);

                await _unitOfWork.ConfirmarTransaccionAsync();
                return Resultado<GrupoDTO>.Success(grupoDTO);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RevertirTransaccionAsync();
                return Resultado<GrupoDTO>.Failure(ErroresCrud.ErrorDeExcepcion("Servicio.CrearGrupo", ex.Message));
            }
        }

        public async Task<Resultado<bool>> EliminarGrupo(int id)
        {
            try
            {
                var resultado = await _repoGrupo.EliminarAsync(id);

                if (resultado.TieneErrores) return Resultado<bool>.Failure(resultado.Errores);

                return Resultado<bool>.Success(resultado.Valor);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion("Servicio.EliminarGrupo", ex.Message));
            }
        }

        public async Task<Resultado<SolicitudDTO>> EnviarSolicitudIngresoAGrupo(EnviarSolicitudDTO solicitud)
        {
            try
            {
                SolicitudUnionGrupo solicitudUnion = _mapper.Map<SolicitudUnionGrupo>(solicitud);

                var resultado_usuarioSolicitante = await _repoUsuarios.ObtenerPorIdAsync(solicitud.UsuarioSolicitanteId);
                var resultado_usuarioAdmin = await _repoUsuarios.ObtenerPorIdAsync(solicitud.UsuarioAdministradorGrupoId);

                if (resultado_usuarioSolicitante.TieneErrores) return Resultado<SolicitudDTO>.Failure(new Error("El usuario solicitante no existe"));
                if (resultado_usuarioAdmin.TieneErrores) return Resultado<SolicitudDTO>.Failure(new Error("El usuario administrador no existe"));

                solicitudUnion.EnviarSolicitudParaUnirseAGrupo(resultado_usuarioSolicitante.Valor, resultado_usuarioAdmin.Valor);
                
                var solicitudEnviada = await _repositorioSolicitud.CrearAsync(solicitudUnion);

                if (solicitudEnviada.TieneErrores) return Resultado<SolicitudDTO>.Failure(solicitudEnviada.Errores);

                var solicitudDTO = _mapper.Map<SolicitudDTO>(solicitudEnviada.Valor);

                return Resultado<SolicitudDTO>.Success(solicitudDTO);

            }
            catch (Exception ex)
            {
                return Resultado<SolicitudDTO>.Failure(ErroresCrud.ErrorDeExcepcion("Servicio.EnviarSolicitudIngresoAGrupo", ex.Message));
            }
        }

        private async Task<Resultado<bool>> IngresarAGrupo(int grupoId, string usuarioSolicitante)
        {
            try
            {
                //await _unitOfWork.IniciarTransaccionAsync();

                var resultadoUsuario = await _repoUsuarios.ObtenerPorIdAsync(usuarioSolicitante);
                
                if (resultadoUsuario.TieneErrores) return Resultado<bool>.Failure(resultadoUsuario.Errores);

                var usuario = resultadoUsuario.Valor;

                var grupoBuscado = await _repoGrupo.ObtenerPorIdAsync(grupoId);

                if (grupoBuscado.TieneErrores) return Resultado<bool>.Failure(grupoBuscado.Errores);

                Grupo grupo = grupoBuscado.Valor;

                usuario.UnirseAGrupo(grupo);
                
                var resultadoActualizacionMiembro = await _repoUsuarios.ActualizarAsync(usuario);

                grupo.AgregarMiembro(usuario);
                var resultadoActualizacionGrupo = await _repoGrupo.ActualizarAsync(grupo);

                if (resultadoActualizacionGrupo.EsCorrecto && resultadoActualizacionMiembro.EsCorrecto)
                {
                    //await _unitOfWork.ConfirmarTransaccionAsync();
                    return Resultado<bool>.Success(true);
                }

                var errores = resultadoActualizacionGrupo.Errores.Concat(resultadoActualizacionMiembro.Errores);
                //await _unitOfWork.RevertirTransaccionAsync();
                return Resultado<bool>.Failure(errores.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Resultado<GrupoDTO>> ObtenerGrupoPorId(int id)
        {
            try
            {
                var resultado = await _repoGrupo.ObtenerPorIdAsync(id);

                if (resultado.TieneErrores) return Resultado<GrupoDTO>.Failure(resultado.Errores);

                var grupoDTO = _mapper.Map<GrupoDTO>(resultado.Valor);

                return Resultado<GrupoDTO>.Success(grupoDTO);
            }
            catch (Exception ex)
            {
                return Resultado<GrupoDTO>.Failure(ErroresCrud.ErrorDeExcepcion("Servicio.ObtenerGrupoPorId", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<GrupoDTO>>> ObtenerTodosLosGrupos()
        {
            try
            {
                var resultado = await _repoGrupo.ObtenerTodosAsync();

                if (resultado.TieneErrores) return Resultado<IEnumerable<GrupoDTO>>.Failure(resultado.Errores);

                var gruposDTO = _mapper.Map<IEnumerable<GrupoDTO>>(resultado.Valor);

                return Resultado<IEnumerable<GrupoDTO>>.Success(gruposDTO);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<GrupoDTO>>.Failure(ErroresCrud.ErrorDeExcepcion("Servicio.ObtenerTodasLasGrupos", ex.Message));
            }
        }

        public async Task<Resultado<bool>> RechazarSolicitudIngresoAGrupo(int idSolicitud)
        {
            try
            {

                var solicitudBuscada = await _repositorioSolicitud.ObtenerPorIdAsync(idSolicitud);

                if (solicitudBuscada.TieneErrores) return Resultado<bool>.Failure(solicitudBuscada.Errores);

                SolicitudUnionGrupo solicitudParaRechazar = solicitudBuscada.Valor;

                if (solicitudParaRechazar.Estado != "Pendiente") return Resultado<bool>.Failure(ErroresSolicitud.Estado_No_Es_Pendiente);

                var solicitudRechazada = await _repositorioSolicitud.RechazarSolicitud(idSolicitud);

                if (solicitudRechazada.TieneErrores) return Resultado<bool>.Failure(solicitudRechazada.Errores);

                return Resultado<bool>.Success(solicitudRechazada.EsCorrecto);

            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion("Servicio.RechazarSolicitudIngresoAGrupo", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<SolicitudDTO>>> ObtenerSolicitudesPorAdministrador(string idAdministrador, string estado)
        {
            try
            {
                var resultado = await _repositorioSolicitud.ObtenerTodasPorAdministrador(idAdministrador,estado);

                if (resultado.TieneErrores) return Resultado<IEnumerable<SolicitudDTO>>.Failure(resultado.Errores);

                var solicitudesDTO = _mapper.Map<IEnumerable<SolicitudDTO>>(resultado.Valor);

                return Resultado<IEnumerable<SolicitudDTO>>.Success(solicitudesDTO);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<SolicitudDTO>>.Failure(ErroresCrud.ErrorDeExcepcion("Servicio.ObtenerTodasLasGrupos", ex.Message));
            }
        }

        public async Task<Resultado<bool>> IngresoAGrupoConCodigo(UnirseAGrupoDTO acceso)
        {
            try
            {
                var grupo = await _repoGrupo.ObtenerPorIdAsync(acceso.GrupoGastoId);

                if (grupo.TieneErrores) return Resultado<bool>.Failure(grupo.Errores);

                var miembroEsIntegrante = await _repoGrupo.MiembroExisteEnElGrupo(grupo.Valor.Id, acceso.UsuarioId);

                if (miembroEsIntegrante.TieneErrores) return Resultado<bool>.Failure(miembroEsIntegrante.Errores);

                var codigoVerificado = grupo.Valor.VerificarCodigo(acceso.Codigo);

                if(codigoVerificado.TieneErrores) return Resultado<bool>.Failure(codigoVerificado.Errores);

                var usuarioIngresado = await IngresarAGrupo(grupo.Valor.Id, acceso.UsuarioId);

                if (usuarioIngresado.EsCorrecto)
                {
                    return Resultado<bool>.Success(usuarioIngresado.EsCorrecto);
                }

                return Resultado<bool>.Failure(usuarioIngresado.Errores);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion("Servicio.IngresoAGrupoConCodigo", ex.Message));
            }
        }
    }
}
