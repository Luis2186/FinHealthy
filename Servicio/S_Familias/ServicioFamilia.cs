using AutoMapper;
using Dominio;
using Dominio.Abstracciones;
using Dominio.Errores;
using Dominio.Familias;
using Dominio.Solicitudes;
using Dominio.Usuarios;
using Microsoft.EntityFrameworkCore;
using Repositorio.Repositorios;
using Repositorio.Repositorios.R_Familias;
using Repositorio.Repositorios.Solicitudes;
using Repositorio.Repositorios.Usuarios;
using Servicio.DTOS.FamiliasDTO;
using Servicio.DTOS.SolicitudesDTO;
using System.Transactions;

namespace Servicio.S_Familias
{
    public class ServicioFamilia : IServicioFamilia
    {
        private readonly IRepositorioUsuario _repoUsuarios;
        private readonly IRepositorioFamilia _repoFamilia;
        private readonly IRepositorioMiembroFamilia _repoMiembroFamilia;
        private readonly IRepositorioSolicitud _repositorioSolicitud;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ServicioFamilia(IRepositorioFamilia repoGrupoFamilia,
                               IRepositorioMiembroFamilia repoMiembroFamilia,
                               IRepositorioUsuario repositorioUsuario,
                               IRepositorioSolicitud repositorioSolicitud,
                               IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repoFamilia = repoGrupoFamilia;
            _repoMiembroFamilia = repoMiembroFamilia;
            _repoUsuarios = repositorioUsuario;
            _repositorioSolicitud = repositorioSolicitud;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Resultado<bool>> AceptarSolicitudIngresoAFamilia(int idSolicitud)
        {
            try
            {
                var solicitudBuscada = await _repositorioSolicitud.ObtenerPorIdAsync(idSolicitud);

                if (solicitudBuscada.TieneErrores) return Resultado<bool>.Failure(solicitudBuscada.Errores);

                SolicitudUnionFamilia solicitudParaAceptar = solicitudBuscada.Valor;

                if (solicitudParaAceptar.Estado != "Pendiente") return Resultado<bool>.Failure(ErroresSolicitud.Estado_No_Es_Pendiente);

                var familiaAdmin = await _repoFamilia.ObtenerFamiliaPorIdAdministrador(solicitudParaAceptar.UsuarioAdministradorGrupoId);
                
                if(familiaAdmin.TieneErrores) return Resultado<bool>.Failure(familiaAdmin.Errores);

                var solicitudAceptada =await _repositorioSolicitud.AceptarSolicitud(idSolicitud);

                if (solicitudAceptada.TieneErrores) return Resultado<bool>.Failure(solicitudAceptada.Errores);

                var usuarioIngresado =await IngresarAFamilia(familiaAdmin.Valor.Id, solicitudParaAceptar.UsuarioSolicitanteId);

                if(usuarioIngresado.EsCorrecto)
                {              
                    return Resultado<bool>.Success(usuarioIngresado.EsCorrecto);
                }

                return Resultado<bool>.Failure(usuarioIngresado.Errores);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion("Servicio.AceptarSolicitudIngresoAFamilia", ex.Message));
            }
        }

        public async Task<Resultado<FamiliaDTO>> ActualizarFamilia(int familiaId,ActualizarFamiliaDTO familiaActDTO)
        {
            try
            {
                var familiaBuscada = await _repoFamilia.ObtenerPorIdAsync(familiaId);

                if (familiaBuscada.TieneErrores) return Resultado<FamiliaDTO>.Failure(familiaBuscada.Errores);

                _mapper.Map(familiaActDTO, familiaBuscada.Valor); // Actualizar el usuario con el DTO

                var resultado = await _repoFamilia.ActualizarAsync(familiaBuscada.Valor);

                if (resultado.TieneErrores) return Resultado<FamiliaDTO>.Failure(resultado.Errores);

                var familiaDTO = _mapper.Map<FamiliaDTO>(resultado.Valor);

                return Resultado<FamiliaDTO>.Success(familiaDTO);
            }
            catch (Exception ex)
            {
                return Resultado<FamiliaDTO>.Failure(ErroresCrud.ErrorDeExcepcion("Servicio.ActualizarFamilia", ex.Message));
            }
        }

        public async Task<Resultado<FamiliaDTO>> CrearFamilia(CrearFamiliaDTO familiaCreacionDTO)
        {
            try
            {
                var usuarioAdmin =await _repoUsuarios.ObtenerPorIdAsync(familiaCreacionDTO.UsuarioAdministradorId);

                if (usuarioAdmin.TieneErrores) return Resultado<FamiliaDTO>.Failure(usuarioAdmin.Errores);

                var familia = _mapper.Map<Familia>(familiaCreacionDTO);

                familia.UsuarioAdministrador = usuarioAdmin.Valor;
                familia.EstablecerCodigo(familiaCreacionDTO.CodigoAcceso);

                var resultado = await _repoFamilia.CrearAsync(familia);

                if (resultado.TieneErrores) return Resultado<FamiliaDTO>.Failure(resultado.Errores);

                var familiaDTO = _mapper.Map<FamiliaDTO>(resultado.Valor);

                return Resultado<FamiliaDTO>.Success(familiaDTO);
            }
            catch (Exception ex)
            {
                return Resultado<FamiliaDTO>.Failure(ErroresCrud.ErrorDeExcepcion("Servicio.CrearFamilia", ex.Message));
            }
        }

        public async Task<Resultado<bool>> EliminarFamilia(int id)
        {
            try
            {
                var resultado = await _repoFamilia.EliminarAsync(id);

                if (resultado.TieneErrores) return Resultado<bool>.Failure(resultado.Errores);

                return Resultado<bool>.Success(resultado.Valor);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion("Servicio.EliminarFamilia", ex.Message));
            }
        }

        public async Task<Resultado<SolicitudDTO>> EnviarSolicitudIngresoAFamilia(EnviarSolicitudDTO solicitud)
        {
            try
            {
                SolicitudUnionFamilia solicitudUnion = _mapper.Map<SolicitudUnionFamilia>(solicitud);

                var resultado_usuarioSolicitante = await _repoUsuarios.ObtenerPorIdAsync(solicitud.UsuarioSolicitanteId);
                var resultado_usuarioAdmin = await _repoUsuarios.ObtenerPorIdAsync(solicitud.UsuarioAdministradorGrupoId);

                if (resultado_usuarioSolicitante.TieneErrores) return Resultado<SolicitudDTO>.Failure(new Error("El usuario solicitante no existe"));
                if (resultado_usuarioAdmin.TieneErrores) return Resultado<SolicitudDTO>.Failure(new Error("El usuario administrador no existe"));

                solicitudUnion.EnviarSolicitudParaUnirseAFamilia(resultado_usuarioSolicitante.Valor, resultado_usuarioAdmin.Valor);
                
                var solicitudEnviada = await _repositorioSolicitud.CrearAsync(solicitudUnion);

                if (solicitudEnviada.TieneErrores) return Resultado<SolicitudDTO>.Failure(solicitudEnviada.Errores);

                var solicitudDTO = _mapper.Map<SolicitudDTO>(solicitudEnviada.Valor);

                return Resultado<SolicitudDTO>.Success(solicitudDTO);

            }
            catch (Exception ex)
            {
                return Resultado<SolicitudDTO>.Failure(ErroresCrud.ErrorDeExcepcion("Servicio.EnviarSolicitudIngresoAFamilia", ex.Message));
            }
        }

        private async Task<Resultado<bool>> IngresarAFamilia(int familiaId, string usuarioSolicitante)
        {
            try
            {
                //await _unitOfWork.IniciarTransaccionAsync();

                var miembroBuscado = await _repoMiembroFamilia.ObtenerPorUsuarioId(usuarioSolicitante);

                if (miembroBuscado.TieneErrores) return Resultado<bool>.Failure(miembroBuscado.Errores);

                var familiaBuscada = await _repoFamilia.ObtenerPorIdAsync(familiaId);

                if (familiaBuscada.TieneErrores) return Resultado<bool>.Failure(familiaBuscada.Errores);

                Familia familia = familiaBuscada.Valor;
                MiembroFamilia miembro = miembroBuscado.Valor;
     
                miembro.UnirserAGrupoFamiliar(familia);
                var resultadoActualizacionMiembro = await _repoMiembroFamilia.ActualizarAsync(miembro);

                familia.AgregarMiembroAFamilia(miembro);
                var resultadoActualizacionFamilia = await _repoFamilia.ActualizarAsync(familia);
                


                if (resultadoActualizacionFamilia.EsCorrecto && resultadoActualizacionMiembro.EsCorrecto)
                {
                    //await _unitOfWork.ConfirmarTransaccionAsync();
                    return Resultado<bool>.Success(true);
                }

                var errores = resultadoActualizacionFamilia.Errores.Concat(resultadoActualizacionMiembro.Errores);
                //await _unitOfWork.RevertirTransaccionAsync();
                return Resultado<bool>.Failure(errores.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Resultado<FamiliaDTO>> ObtenerFamiliaPorId(int id)
        {
            try
            {
                var resultado = await _repoFamilia.ObtenerPorIdAsync(id);

                if (resultado.TieneErrores) return Resultado<FamiliaDTO>.Failure(resultado.Errores);

                var familiaDTO = _mapper.Map<FamiliaDTO>(resultado.Valor);

                return Resultado<FamiliaDTO>.Success(familiaDTO);
            }
            catch (Exception ex)
            {
                return Resultado<FamiliaDTO>.Failure(ErroresCrud.ErrorDeExcepcion("Servicio.ObtenerFamiliaPorId", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<FamiliaDTO>>> ObtenerTodasLasFamilias()
        {
            try
            {
                var resultado = await _repoFamilia.ObtenerTodosAsync();

                if (resultado.TieneErrores) return Resultado<IEnumerable<FamiliaDTO>>.Failure(resultado.Errores);

                var familiasDTO = _mapper.Map<IEnumerable<FamiliaDTO>>(resultado.Valor);

                return Resultado<IEnumerable<FamiliaDTO>>.Success(familiasDTO);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<FamiliaDTO>>.Failure(ErroresCrud.ErrorDeExcepcion("Servicio.ObtenerTodasLasFamilias", ex.Message));
            }
        }

        public async Task<Resultado<bool>> RechazarSolicitudIngresoAFamilia(int idSolicitud)
        {
            try
            {

                var solicitudBuscada = await _repositorioSolicitud.ObtenerPorIdAsync(idSolicitud);

                if (solicitudBuscada.TieneErrores) return Resultado<bool>.Failure(solicitudBuscada.Errores);

                SolicitudUnionFamilia solicitudParaRechazar = solicitudBuscada.Valor;

                if (solicitudParaRechazar.Estado != "Pendiente") return Resultado<bool>.Failure(ErroresSolicitud.Estado_No_Es_Pendiente);

                var solicitudRechazada = await _repositorioSolicitud.RechazarSolicitud(idSolicitud);

                if (solicitudRechazada.TieneErrores) return Resultado<bool>.Failure(solicitudRechazada.Errores);

                return Resultado<bool>.Success(solicitudRechazada.EsCorrecto);

            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion("Servicio.RechazarSolicitudIngresoAFamilia", ex.Message));
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
                return Resultado<IEnumerable<SolicitudDTO>>.Failure(ErroresCrud.ErrorDeExcepcion("Servicio.ObtenerTodasLasFamilias", ex.Message));
            }
        }

        public async Task<Resultado<bool>> IngresoAFamiliaConCodigo(UnirseAFamiliaDTO acceso)
        {
            try
            {
                var familia = await _repoFamilia.ObtenerPorIdAsync(acceso.FamiliaId);

                if (familia.TieneErrores) return Resultado<bool>.Failure(familia.Errores);

                var miembroEsIntegrante = await _repoFamilia.MiembroExisteEnLaFamilia(familia.Valor.Id, acceso.UsuarioId);

                if (miembroEsIntegrante.TieneErrores) return Resultado<bool>.Failure(miembroEsIntegrante.Errores);

                var codigoVerificado = familia.Valor.VerificarCodigo(acceso.Codigo);

                if(codigoVerificado.TieneErrores) return Resultado<bool>.Failure(codigoVerificado.Errores);

                var usuarioIngresado = await IngresarAFamilia(familia.Valor.Id, acceso.UsuarioId);

                if (usuarioIngresado.EsCorrecto)
                {
                    return Resultado<bool>.Success(usuarioIngresado.EsCorrecto);
                }

                return Resultado<bool>.Failure(usuarioIngresado.Errores);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion("Servicio.IngresoAFamiliaConCodigo", ex.Message));
            }
        }
    }
}
