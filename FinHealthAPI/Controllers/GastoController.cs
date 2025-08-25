using AutoMapper;
using Dominio;
using Dominio.Gastos;
using Dominio.Usuarios;
using Servicio.DTOS.GastosDTO; // CrearGastoDTO
using Servicio.DTOS.GruposDTO; // GrupoDTO
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Servicio.S_Gastos;
using Servicio.Usuarios;
using System.Configuration;
using System.Net;

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


        // Crear un nuevo usuario
        /// <summary>
        /// Crea un nuevo gasto.
        /// </summary>
        /// <param name="gastoCreacionDTO">Datos del gasto a crear.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Un objeto GrupoDTO con los detalles del grupo creado.</returns>
        /// <response code="200">Devuelve el ID del gasto creado.</response>
        /// <response code="400">Si hay errores de validación.</response>
        /// <response code="404">Si ya existe un gasto con ese ID.</response>
        [HttpPost("crear")]
        public async Task<ActionResult<GastoDTO>> CrearGasto([FromBody] CrearGastoDTO gastoCreacionDTO, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = 400,
                    Title = "Error de validacion",
                    Detail = "El cuerpo de la solicitud es invalido, contiene errores de validacion.",
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                      ["errors"] = ModelState.Keys
                            .SelectMany(key => ModelState[key].Errors.Select(error => new
                            {
                                Code = key, // Aquí puedes ajustar el código como desees
                                Description = error.ErrorMessage
                            }))
                    }
                });
            }

            // Obtiene el usuario actual del token JWT
            var usuarioActualId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(usuarioActualId))
                return Unauthorized("No se pudo identificar el usuario actual.");

            var resultado = await _servicioGasto.CrearGasto(gastoCreacionDTO, usuarioActualId, cancellationToken);

            if (!resultado.EsCorrecto)
            {
                return Conflict(new ProblemDetails
                {
                    Title = "Error al ingresar el gasto",
                    Detail = "Ha ocurrido un error al intentar crear el gasto",
                    Status = 400,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = resultado.Errores
                    }
                });
            }

            return Ok(resultado.Valor);
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
        public async Task<ActionResult<GastosSegmentadosDTO>> ObtenerGastosSegmentados(
            [FromQuery] int grupoId, [FromQuery] int? anio, [FromQuery] int? mes,
            [FromQuery] TipoGasto? tipoGasto, CancellationToken cancellationToken)
        {
            var usuarioActualId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(usuarioActualId))
                return Unauthorized("No se pudo identificar el usuario actual.");

            var tipoGastoFinal = tipoGasto ?? TipoGasto.Todos;
            var resultado = await _servicioGasto.ObtenerGastosSegmentados(grupoId, anio, mes, usuarioActualId, tipoGastoFinal, cancellationToken);

            if (!resultado.EsCorrecto)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Error al obtener los gastos segmentados",
                    Detail = "Ha ocurrido un error al intentar obtener los gastos segmentados",
                    Status = 400,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = resultado.Errores
                    }
                });
            }

            return Ok(resultado.Valor);
        }
    }
}
