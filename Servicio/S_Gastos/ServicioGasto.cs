using AutoMapper;
using Dominio.Abstracciones;
using Dominio;
using Dominio.Gastos;
using Dominio.Grupos;
using Dominio.Usuarios;
using Repositorio.Repositorios.R_Gastos.R_Monedas;
using Repositorio.Repositorios.R_Categoria;
using Repositorio.Repositorios.R_Categoria.R_SubCategoria;
using Repositorio.Repositorios.R_Gastos;
using Repositorio.Repositorios.R_Gastos.R_MetodosDePago;
using Repositorio.Repositorios.Usuarios;
using Servicio.DTOS.GastosDTO;
using Servicio.DTOS.GruposDTO;
using Servicio.S_Categorias;
using Servicio.S_Categorias.S_SubCategorias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Repositorio.Repositorios.R_Grupo;

namespace Servicio.S_Gastos
{
    public class ServicioGasto : IServicioGasto
    {
        private readonly IRepositorioSubCategoria _repoSubCategoria;
        private readonly IRepositorioMoneda _repoMoneda;
        private readonly IRepositorioMetodoDePago _repoMetodoDePago;
        private readonly IRepositorioUsuario _repoUsuarios;
        private readonly IRepositorioGasto _repoGastos;
        private readonly IRepositorioGrupo _repoGrupo;
        private readonly IRepositorioGrupoSubCategoria _repoGrupoSubCategoria;
        private readonly IMapper _mapper;

        public ServicioGasto(
            IRepositorioSubCategoria repoSubCategoria,
            IRepositorioMoneda repoMoneda,
            IRepositorioMetodoDePago repoMetodoDePago,
            IRepositorioUsuario repoUsuario,
            IRepositorioGasto repoGastos,
            IMapper mapper,
            IRepositorioGrupo repoGrupo,
            IRepositorioGrupoSubCategoria repoGrupoSubCategoria)
        {
            _repoSubCategoria = repoSubCategoria;
            _repoMoneda = repoMoneda;
            _repoMetodoDePago = repoMetodoDePago;
            _repoUsuarios = repoUsuario;
            _repoGastos = repoGastos;
            _mapper = mapper;
            _repoGrupo = repoGrupo;
            _repoGrupoSubCategoria = repoGrupoSubCategoria;
        }

        public async Task<Resultado<GastoDTO>> CrearGasto(CrearGastoFijoDTO gastoCreacionDTO, string usuarioActualId, CancellationToken cancellationToken)
            => await CrearGastoInterno(gastoCreacionDTO, usuarioActualId, cancellationToken);

        public async Task<Resultado<GastoDTO>> CrearGasto(CrearGastoMensualDTO gastoCreacionDTO, string usuarioActualId, CancellationToken cancellationToken)
            => await CrearGastoInterno(gastoCreacionDTO, usuarioActualId, cancellationToken);

        public async Task<Resultado<GastoDTO>> CrearGasto(CrearGastoCompartidoDTO gastoCreacionDTO, string usuarioActualId, CancellationToken cancellationToken)
            => await CrearGastoInterno(gastoCreacionDTO, usuarioActualId, cancellationToken);

        public async Task<Resultado<GastoDTO>> CrearGasto(CrearGastoEnCuotasDTO gastoCreacionDTO, string usuarioActualId, CancellationToken cancellationToken)
            => await CrearGastoInterno(gastoCreacionDTO, usuarioActualId, cancellationToken);

        private async Task<Resultado<GastoDTO>> CrearGastoInterno(CrearGastoBaseDTO gastoCreacionDTO, string usuarioActualId, CancellationToken cancellationToken)
        {
            var resultadoNuevoGasto = await FormarGasto(gastoCreacionDTO, usuarioActualId, cancellationToken);

            if (resultadoNuevoGasto.TieneErrores)
                return Resultado<GastoDTO>.Failure(resultadoNuevoGasto.Errores);

            var nuevoGasto = resultadoNuevoGasto.Valor;
            var resultado = await _repoGastos.CrearAsync(nuevoGasto, cancellationToken);

            if (resultado.TieneErrores)
                return Resultado<GastoDTO>.Failure(resultado.Errores);

            var gastoDto = _mapper.Map<GastoDTO>(resultado.Valor);
            return gastoDto;
        }

        public async Task<Resultado<GastoDTO>> ActualizarGasto(CrearGastoBaseDTO gastoActualizacionDTO,
            string usuarioActualId, CancellationToken cancellationToken)
        {
            var gastoResult = await _repoGastos.ObtenerPorIdAsync(gastoActualizacionDTO.GrupoId, cancellationToken);

            if (gastoResult.TieneErrores)
                return Resultado<GastoDTO>.Failure(gastoResult.Errores);

            var gastoEncontrado = gastoResult.Valor;

            var resultadoActualizacionGasto = await FormarGasto(gastoActualizacionDTO,
                usuarioActualId, cancellationToken, gastoEncontrado);

            if (resultadoActualizacionGasto.TieneErrores)
                return Resultado<GastoDTO>.Failure(resultadoActualizacionGasto.Errores);

            var gastoActualizado = resultadoActualizacionGasto.Valor;
            var resultadoActualizacion = await _repoGastos.ActualizarAsync(gastoActualizado, cancellationToken);

            if (resultadoActualizacion.TieneErrores)
                return Resultado<GastoDTO>.Failure(resultadoActualizacion.Errores);

            var gastoDto = _mapper.Map<GastoDTO>(resultadoActualizacion.Valor);
            return gastoDto;
        }

        private async Task<Resultado<Gasto>> FormarGasto(CrearGastoBaseDTO dto,
            string usuarioActualId, CancellationToken cancellationToken,
            Gasto gastoExistente = null)
        {
            // Guard clauses y validaciones críticas
            var grupoResult = await _repoGrupo.ObtenerGrupoPorIdConUsuariosYSubcategorias(dto.GrupoId, cancellationToken);
            if (grupoResult.TieneErrores)
                return Resultado<Gasto>.Failure(grupoResult.Errores);
            var grupo = grupoResult.Valor;
            if (grupo == null)
                return Resultado<Gasto>.Failure(new Error("Grupo no encontrado", "No se encontró el grupo especificado."));
            if (!grupo.MiembrosGrupoGasto.Any(u => u.Id == usuarioActualId))
                return Resultado<Gasto>.Failure(new Error("Permiso denegado", "Solo los miembros del grupo pueden crear/actualizar gastos."));

            var grupoSubCategoriaResult = await _repoGrupoSubCategoria.ObtenerPorGrupoYSubCategoriaAsync(dto.GrupoId, dto.SubCategoriaId, cancellationToken);
            if (grupoSubCategoriaResult.TieneErrores)
                return Resultado<Gasto>.Failure(grupoSubCategoriaResult.Errores);
            var grupoSubCategoria = grupoSubCategoriaResult.Valor;
            if (grupoSubCategoria == null || !grupoSubCategoria.Activo)
                return Resultado<Gasto>.Failure(new Error("Permiso denegado", "La subcategoría no está habilitada para este grupo."));

            if (dto.UsuarioCreadorId != usuarioActualId)
                return Resultado<Gasto>.Failure(new Error("Permiso denegado", "Solo puedes crear/actualizar gastos personales para ti mismo."));

            var metodoDePagoResult = await _repoMetodoDePago.ObtenerPorIdAsync(dto.MetodoDePagoId, cancellationToken);
            if (metodoDePagoResult.TieneErrores)
                return Resultado<Gasto>.Failure(metodoDePagoResult.Errores);
            var monedaResult = await _repoMoneda.ObtenerPorCodigoAsync(dto.MonedaId, cancellationToken);
            if (monedaResult.TieneErrores)
                return Resultado<Gasto>.Failure(monedaResult.Errores);
            var usuarioCreadorResult = await _repoUsuarios.ObtenerPorIdAsync(dto.UsuarioCreadorId, cancellationToken);
            if (usuarioCreadorResult.TieneErrores)
                return Resultado<Gasto>.Failure(usuarioCreadorResult.Errores);

            MetodoDePago metodoDePagoElegido = metodoDePagoResult.Valor;
            SubCategoria subCategoriaElegida = grupoSubCategoria.SubCategoria;
            Moneda monedaElegida = monedaResult.Valor;
            Usuario usuarioCreador = usuarioCreadorResult.Valor;

            // Factoría de gastos
            var gastoResult = CrearGastoDesdeDTO(dto, grupo, subCategoriaElegida, metodoDePagoElegido, monedaElegida, usuarioCreador);
            if (gastoResult.TieneErrores)
                return Resultado<Gasto>.Failure(gastoResult.Errores);

            var nuevoGasto = gastoResult.Valor;

            // Validación de negocio
            if (!(nuevoGasto is GastoCompartidoPrincipal))
            {
                var resultadoValidacion = nuevoGasto.EsValido();
                if (resultadoValidacion.TieneErrores)
                    return Resultado<Gasto>.Failure(resultadoValidacion.Errores);
            }

            var validacionConsistencia = grupo.ValidarConsistenciaSubcategoria(nuevoGasto);
            if (!validacionConsistencia.EsCorrecto)
                return Resultado<Gasto>.Failure(validacionConsistencia.Errores);

            if (gastoExistente != null)
            {
                gastoExistente.ActualizarGasto(nuevoGasto);
                return gastoExistente;
            }

            return nuevoGasto;
        }

        private Resultado<Gasto> CrearGastoDesdeDTO(CrearGastoBaseDTO dto, Grupo grupo, SubCategoria subCategoria, MetodoDePago metodoDePago, Moneda moneda, Usuario usuarioCreador)
        {
            switch (dto)
            {
                case CrearGastoFijoDTO fijo:
                    return Resultado<Gasto>.Success(new GastoFijo(subCategoria, metodoDePago, moneda, fijo.FechaDeGasto, fijo.Descripcion, fijo.Etiqueta, fijo.Lugar, fijo.Monto, grupo, usuarioCreador)
                    {
                        Periodicidad = fijo.Periodicidad,
                        FechaInicio = fijo.FechaInicio,
                        FechaFin = fijo.FechaFin
                    });
                case CrearGastoMensualDTO mensual:
                    return Resultado<Gasto>.Success(new GastoMensual(subCategoria, metodoDePago, moneda, mensual.FechaDeGasto, mensual.Descripcion, mensual.Etiqueta, mensual.Lugar, mensual.Monto, grupo, usuarioCreador));
                case CrearGastoCompartidoDTO compartido:
                    return CrearGastoCompartido(compartido, grupo, subCategoria, metodoDePago, moneda, usuarioCreador);
                case CrearGastoEnCuotasDTO cuotas:
                    var gastoEnCuotas = new GastoEnCuotas(subCategoria, metodoDePago, moneda, cuotas.FechaDeGasto, cuotas.Descripcion, cuotas.Etiqueta, cuotas.Lugar, cuotas.Monto, cuotas.CantidadDeCuotas, grupo, usuarioCreador);
                    var resultadoCuotas = gastoEnCuotas.GenerarCuotas(cuotas.CantidadDeCuotas);
                    if (resultadoCuotas.TieneErrores) return Resultado<Gasto>.Failure(resultadoCuotas.Errores);
                    return Resultado<Gasto>.Success(gastoEnCuotas);
                default:
                    return Resultado<Gasto>.Failure(new Error("Tipo de gasto no soportado", "El tipo de DTO recibido no es válido para la creación de gastos."));
            }
        }

        private Resultado<Gasto> CrearGastoCompartido(
            CrearGastoCompartidoDTO dto,
            Grupo grupo,
            SubCategoria subCategoria,
            MetodoDePago metodoDePago,
            Moneda moneda,
            Usuario usuarioCreador)
        {
            var participantesIds = dto.ObtenerParticipantesIds();
            var participantes = grupo.MiembrosGrupoGasto.Where(u => participantesIds.Contains(u.Id)).ToList();
            var gastoCompartido = new GastoCompartidoPrincipal(subCategoria, metodoDePago, moneda, dto.FechaDeGasto, dto.Descripcion, dto.Etiqueta, dto.Lugar, dto.Monto, grupo, usuarioCreador);
            var resultadoGeneracion = gastoCompartido.GenerarGastosCompartidos(participantes, dto.PorcentajesCompartidos);
            if (resultadoGeneracion.TieneErrores)
                return Resultado<Gasto>.Failure(resultadoGeneracion.Errores);
            var resultadoValidacion = gastoCompartido.EsValido(participantes);
            if (resultadoValidacion.TieneErrores)
                return Resultado<Gasto>.Failure(resultadoValidacion.Errores);
            return Resultado<Gasto>.Success(gastoCompartido);
        }

        public async Task<Resultado<GastosSegmentadosDTO>> ObtenerGastosSegmentados(
            int grupoId, int? anio, int? mes, string usuarioActualId, TipoGasto tipoGasto, CancellationToken cancellationToken)
        {
            var resultado = new GastosSegmentadosDTO
            {
                GastosFijos = new List<GastoFijoDTO>(),
                GastosMensuales = new List<GastoDTO>(),
                GastosCompartidos = new List<GastoCompartidoDTO>(),
                GastosEnCuotas = new List<GastoCuotaDTO>()
            };
            List<Error> errores = new();

            switch (tipoGasto)
            {
                case TipoGasto.Fijo:
                    resultado.GastosFijos = await ObtenerGastosFijos(grupoId, anio, mes, usuarioActualId, cancellationToken);
                    break;
                case TipoGasto.Mensual:
                    resultado.GastosMensuales = await ObtenerGastosMensuales(grupoId, anio, mes, usuarioActualId, cancellationToken);
                    break;
                case TipoGasto.Compartido:
                    resultado.GastosCompartidos = await ObtenerGastosCompartidos(grupoId, anio, mes, usuarioActualId, cancellationToken);
                    break;
                case TipoGasto.Cuota:
                    resultado.GastosEnCuotas = await ObtenerGastosEnCuotas(grupoId, anio, mes, usuarioActualId, cancellationToken);
                    break;
                case TipoGasto.Todos:
                default:
                    resultado.GastosFijos = await ObtenerGastosFijos(grupoId, anio, mes, usuarioActualId, cancellationToken);
                    resultado.GastosMensuales = await ObtenerGastosMensuales(grupoId, anio, mes, usuarioActualId, cancellationToken);
                    resultado.GastosCompartidos = await ObtenerGastosCompartidos(grupoId, anio, mes, usuarioActualId, cancellationToken);
                    resultado.GastosEnCuotas = await ObtenerGastosEnCuotas(grupoId, anio, mes, usuarioActualId, cancellationToken);
                    break;
            }

            return Resultado<GastosSegmentadosDTO>.Success(resultado);
        }

        private async Task<List<GastoFijoDTO>> ObtenerGastosFijos(int grupoId, int? anio, int? mes, string usuarioActualId, CancellationToken cancellationToken)
        {
            var gastosFijosResult = await _repoGastos.ObtenerGastosPorGrupoYUsuario(grupoId, usuarioActualId, cancellationToken, esFijo: true);
            if (gastosFijosResult.TieneErrores)
                return new List<GastoFijoDTO>();
            var gastosFiltrados = gastosFijosResult.Valor
                .OfType<GastoFijo>()
                .Where(g =>
                    g.Estado && // Solo activos
                    ((!anio.HasValue || !mes.HasValue) ||
                    (
                        g.FechaInicio.HasValue && g.FechaFin.HasValue &&
                        new DateTime(anio.Value, mes.Value, 1) >= g.FechaInicio.Value.Date &&
                        new DateTime(anio.Value, mes.Value, 1) <= g.FechaFin.Value.Date
                    ))
                )
                .ToList();
            return _mapper.Map<List<GastoFijoDTO>>(gastosFiltrados);
        }

        private async Task<List<GastoDTO>> ObtenerGastosMensuales(int grupoId, int? anio, int? mes, string usuarioActualId, CancellationToken cancellationToken)
        {
            var gastosResult = await _repoGastos.ObtenerGastosPorGrupoYUsuario(grupoId, usuarioActualId, cancellationToken, esFijo: false);
            if (gastosResult.TieneErrores)
                return new List<GastoDTO>();
            var gastosFiltrados = gastosResult.Valor
                .OfType<GastoMensual>()
                .Where(g =>
                    g.Estado && // Solo activos
                    (!anio.HasValue || g.FechaDeGasto.Year == anio.Value) && (!mes.HasValue || g.FechaDeGasto.Month == mes.Value)
                )
                .ToList();
            return _mapper.Map<List<GastoDTO>>(gastosFiltrados);
        }

        private async Task<List<GastoCompartidoDTO>> ObtenerGastosCompartidos(int grupoId, int? anio, int? mes, string usuarioActualId, CancellationToken cancellationToken)
        {
            var gastosCompartidosResult = await _repoGastos.ObtenerGastosCompartidosPorGrupoYUsuario(grupoId, usuarioActualId, cancellationToken);
            if (gastosCompartidosResult.TieneErrores)
                return new List<GastoCompartidoDTO>();
            var gastosFiltrados = gastosCompartidosResult.Valor
                .OfType<GastoCompartidoPrincipal>()
                .Where(g =>
                    g.Estado && // Solo activos
                    (!anio.HasValue || g.FechaDeGasto.Year == anio.Value) && (!mes.HasValue || g.FechaDeGasto.Month == mes.Value)
                )
                .ToList();
            return _mapper.Map<List<GastoCompartidoDTO>>(gastosFiltrados);
        }

        private async Task<List<GastoCuotaDTO>> ObtenerGastosEnCuotas(int grupoId, int? anio, int? mes, string usuarioActualId, CancellationToken cancellationToken)
        {
            var gastosEnCuotasResult = await _repoGastos.ObtenerGastosEnCuotasPorGrupoYUsuario(grupoId, usuarioActualId, cancellationToken);
            if (gastosEnCuotasResult.TieneErrores)
                return new List<GastoCuotaDTO>();
            var gastosFiltrados = gastosEnCuotasResult.Valor
                .OfType<GastoEnCuotas>()
                .Where(g =>
                    g.Estado && // Solo activos
                    (!anio.HasValue || g.FechaDeGasto.Year == anio.Value) && (!mes.HasValue || g.FechaDeGasto.Month == mes.Value)
                )
                .ToList();
            return _mapper.Map<List<GastoCuotaDTO>>(gastosFiltrados);
        }
    }
}