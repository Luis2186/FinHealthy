using AutoMapper;
using Dominio;
using Dominio.Gastos;
using Dominio.Usuarios;
using Servicio.DTOS.GastosDTO; // CrearGastoBaseDTO
using Servicio.DTOS.GruposDTO; // GrupoDTO
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Servicio.S_Gastos;
using Servicio.Usuarios;
using System.Configuration;
using System.Net;
using FinHealthAPI.Extensiones;

namespace FinHealthAPI.Controllers
{
    /// <summary>
    /// Controlador para la gestión de gastos.
    /// </summary>
    [Authorize(Roles = "Sys_Adm , Administrador, Usuario")]
    [ApiController]
    [Route("/gasto")]
    public class GastoController : Controller
    {
        private readonly IServicioGasto _servicioGasto;
        private readonly IMapper _mapper;

        public GastoController(IServicioGasto servicioGasto, IMapper mapper)
        {
            _servicioGasto = servicioGasto;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene los gastos segmentados (fijos, compartidos, en cuotas) filtrados por año, mes y tipo de gasto.
        /// </summary>
        /// <param name="grupoId">Id del grupo.</param>
        /// <param name="anio">Año (opcional).</param>
        /// <param name="mes">Mes (opcional).</param>
        /// <param name="tipoGasto">Tipo de gasto: "fijo", "compartido", "cuota" o "todos".</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Gastos segmentados por tipo.</returns>
        [HttpGet("segmentados")]
        [ProducesResponseType(typeof(GastosSegmentadosDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GastosSegmentadosDTO>> ObtenerGastosSegmentados(
            [FromQuery] int grupoId, [FromQuery] int? anio, [FromQuery] int? mes,
            [FromQuery] TipoGasto? tipoGasto, CancellationToken cancellationToken)
        {
            var usuarioActualId = User.GetUserId();
            if (string.IsNullOrEmpty(usuarioActualId))
                return Unauthorized(new ProblemDetails
                {
                    Title = "No autorizado",
                    Detail = "No se pudo identificar el usuario actual.",
                    Status = StatusCodes.Status401Unauthorized,
                    Instance = HttpContext.Request.Path
                });

            var tipoGastoFinal = tipoGasto ?? TipoGasto.Todos;
            var resultado = await _servicioGasto.ObtenerGastosSegmentados(grupoId, anio, mes, usuarioActualId, tipoGastoFinal, cancellationToken);

            if (!resultado.EsCorrecto)
            {
                // Si el error es por pertenencia al grupo, devolver Unauthorized
                var error = resultado.Errores.FirstOrDefault(e => e.ToString().Contains("permisos para ver los gastos de este grupo"));
                if (error != null)
                {
                    return Unauthorized(new ProblemDetails
                    {
                        Title = "Acceso denegado",
                        Detail = error.ToString(),
                        Status = StatusCodes.Status401Unauthorized,
                        Instance = HttpContext.Request.Path
                    });
                }
                return BadRequest(new ProblemDetails
                {
                    Title = "Error al obtener los gastos segmentados",
                    Detail = resultado.ObtenerErroresComoString(),
                    Status = StatusCodes.Status400BadRequest,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = resultado.Errores
                    }
                });
            }

            return Ok(resultado.Valor);
        }

        /// <summary>
        /// Crea un nuevo gasto fijo.
        /// </summary>
        /// <param name="dto">Datos del gasto fijo a crear.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Un objeto GastoDTO con los detalles del gasto fijo creado.</returns>
        /// <response code="201">Devuelve el gasto fijo creado.</response>
        /// <response code="400">Si hay errores de validación.</response>
        /// <response code="401">Si no se identifica el usuario.</response>
        [HttpPost("crear-fijo")]
        [ProducesResponseType(typeof(GastoDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GastoDTO>> CrearGastoFijo([FromBody] CrearGastoFijoDTO dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ProblemDetails
                {
                    Title = "Error de validación",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Errores de validación.",
                    Extensions = { ["errors"] = ModelState }
                });

            var usuarioActualId = User.GetUserId();
            if (string.IsNullOrEmpty(usuarioActualId))
                return Unauthorized(new ProblemDetails
                {
                    Title = "No autorizado",
                    Status = StatusCodes.Status401Unauthorized
                });

            var resultado = await _servicioGasto.CrearGasto(dto, usuarioActualId, cancellationToken);

            if (!resultado.EsCorrecto)
                return BadRequest(new ProblemDetails
                {
                    Title = "Error al ingresar el gasto",
                    Status = StatusCodes.Status400BadRequest,
                    Extensions = { ["errors"] = resultado.Errores }
                });

            return CreatedAtAction(nameof(CrearGastoFijo), new { id = resultado.Valor.Id }, resultado.Valor);
        }

        /// <summary>
        /// Crea un nuevo gasto mensual.
        /// </summary>
        /// <param name="dto">Datos del gasto mensual a crear.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Un objeto GastoDTO con los detalles del gasto mensual creado.</returns>
        /// <response code="201">Devuelve el gasto mensual creado.</response>
        /// <response code="400">Si hay errores de validación.</response>
        /// <response code="401">Si no se identifica el usuario.</response>
        [HttpPost("crear-mensual")]
        [ProducesResponseType(typeof(GastoDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GastoDTO>> CrearGastoMensual([FromBody] CrearGastoMensualDTO dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ProblemDetails { Title = "Error de validación", Status = StatusCodes.Status400BadRequest, Detail = "Errores de validación.", Extensions = { ["errors"] = ModelState } });
            var usuarioActualId = User.GetUserId();
            if (string.IsNullOrEmpty(usuarioActualId))
                return Unauthorized(new ProblemDetails { Title = "No autorizado", Status = StatusCodes.Status401Unauthorized });
            var resultado = await _servicioGasto.CrearGasto(dto, usuarioActualId, cancellationToken);
            if (!resultado.EsCorrecto)
                return BadRequest(new ProblemDetails { Title = "Error al ingresar el gasto", Status = StatusCodes.Status400BadRequest, Extensions = { ["errors"] = resultado.Errores } });
            return CreatedAtAction(nameof(CrearGastoMensual), new { id = resultado.Valor.Id }, resultado.Valor);
        }

        /// <summary>
        /// Crea un nuevo gasto compartido.
        /// </summary>
        /// <param name="dto">Datos del gasto compartido a crear.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Un objeto GastoDTO con los detalles del gasto compartido creado.</returns>
        /// <response code="201">Devuelve el gasto compartido creado.</response>
        /// <response code="400">Si hay errores de validación.</response>
        /// <response code="401">Si no se identifica el usuario.</response>
        [HttpPost("crear-compartido")]
        [ProducesResponseType(typeof(GastoDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GastoDTO>> CrearGastoCompartido([FromBody] CrearGastoCompartidoDTO dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ProblemDetails { Title = "Error de validación", Status = StatusCodes.Status400BadRequest, Detail = "Errores de validación.", Extensions = { ["errors"] = ModelState } });
            var usuarioActualId = User.GetUserId();
            if (string.IsNullOrEmpty(usuarioActualId))
                return Unauthorized(new ProblemDetails { Title = "No autorizado", Status = StatusCodes.Status401Unauthorized });
            var resultado = await _servicioGasto.CrearGasto(dto, usuarioActualId, cancellationToken);
            if (!resultado.EsCorrecto)
                return BadRequest(new ProblemDetails { Title = "Error al ingresar el gasto", Status = StatusCodes.Status400BadRequest, Extensions = { ["errors"] = resultado.Errores } });
            return CreatedAtAction(nameof(CrearGastoCompartido), new { id = resultado.Valor.Id }, resultado.Valor);
        }

        /// <summary>
        /// Crea un nuevo gasto en cuotas.
        /// </summary>
        /// <param name="dto">Datos del gasto en cuotas a crear.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Un objeto GastoDTO con los detalles del gasto en cuotas creado.</returns>
        /// <response code="201">Devuelve el gasto en cuotas creado.</response>
        /// <response code="400">Si hay errores de validación.</response>
        /// <response code="401">Si no se identifica el usuario.</response>
        [HttpPost("crear-cuotas")]
        [ProducesResponseType(typeof(GastoDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GastoDTO>> CrearGastoEnCuotas([FromBody] CrearGastoEnCuotasDTO dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ProblemDetails { Title = "Error de validación", Status = StatusCodes.Status400BadRequest, Detail = "Errores de validación.", Extensions = { ["errors"] = ModelState } });
            var usuarioActualId = User.GetUserId();
            if (string.IsNullOrEmpty(usuarioActualId))
                return Unauthorized(new ProblemDetails { Title = "No autorizado", Status = StatusCodes.Status401Unauthorized });
            var resultado = await _servicioGasto.CrearGasto(dto, usuarioActualId, cancellationToken);
            if (!resultado.EsCorrecto)
                return BadRequest(new ProblemDetails { Title = "Error al ingresar el gasto", Status = StatusCodes.Status400BadRequest, Extensions = { ["errors"] = resultado.Errores } });
            return CreatedAtAction(nameof(CrearGastoEnCuotas), new { id = resultado.Valor.Id }, resultado.Valor);
        }
    }
}
