using AutoMapper;
using Dominio;
using Dominio.Usuarios;
using Servicio.DTOS.SolicitudesDTO; // EnviarSolicitudDTO, SolicitudDTO, PaginacionSolicitudDTO
using Servicio.DTOS.GruposDTO; // GrupoDTO, UnirseAGrupoDTO
using Servicio.S_Grupos; // IServicioGrupos
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Servicio.Usuarios;
using System.Configuration;
using System.Net;

namespace FinHealthAPI.Controllers
{
    /// <summary>
    /// Controlador para la gestión de solicitudes.
    /// </summary>
    [Authorize(Roles = "Sys_Adm , Administrador, Usuario")]
    [ApiController]
    [Route("/solicitud")]
    public class SolicitudController : Controller
    {
        private readonly IServicioGrupos _servicioGrupo;
        private readonly IMapper _mapper;

        public SolicitudController(IServicioGrupos servicioGrupo, IMapper mapper)
        {
            _servicioGrupo = servicioGrupo;
            _mapper = mapper;
        }


        // Obtener todos los usuarios con paginación
        /// <summary>
        /// Obtiene las solicitudes de un administrador en particular.
        /// </summary>
        /// <param name="solicitudes">Objeto que contiene el id del administrador y el estado de las solicitudes.</param>
        /// <returns>Lista de solicitudes del administrador.</returns>
        [HttpGet("porAdmin")]
        [ProducesResponseType(typeof(IEnumerable<SolicitudDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<SolicitudDTO>>> ObtenerSolicitudesPorAdministrador([FromBody] PaginacionSolicitudDTO solicitudes, CancellationToken cancellationToken)
        {
            var resultado = await _servicioGrupo.ObtenerSolicitudesPorAdministrador(solicitudes.IdAdministrador, solicitudes.Estado, cancellationToken);

            if (resultado.TieneErrores)
                return NotFound(new ProblemDetails
                {
                    Title = "Error solicitudes por administrador",
                    Detail = "Hubo un error al intentar obtener las solicitudes por administrador",
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });

            return Ok(resultado.Valor);
        }

        // Crear un nuevo usuario
        /// <summary>
        /// Envía una solicitud para unirse a un grupo.
        /// </summary>
        /// <param name="enviarSolicitudDTO">Objeto que contiene la información de la solicitud.</param>
        /// <returns>El ID de la solicitud creada.</returns>
        [HttpPost("enviar")]
        [ProducesResponseType(typeof(SolicitudDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SolicitudDTO>> EnviarSolicitud([FromBody] EnviarSolicitudDTO enviarSolicitudDTO, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Error de validación",
                    Detail = "El cuerpo de la solicitud es inválido, contiene errores de validación.",
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = ModelState.Keys
                        .SelectMany(key => ModelState[key].Errors.Select(error => new
                        {
                            Code = key,
                            Description = error.ErrorMessage
                        })) }
                });
            }

            var resultado = await _servicioGrupo.EnviarSolicitudIngresoAGrupo(enviarSolicitudDTO, cancellationToken);

            if (resultado.TieneErrores)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Error al enviar la solicitud",
                    Detail = "Ah ocurrido un error cuando se intento enviar la solicitud",
                    Status = StatusCodes.Status400BadRequest,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });
            }

            return CreatedAtAction(nameof(EnviarSolicitud), new { id = resultado.Valor.Id }, resultado.Valor);
        }

        // Crear un nuevo usuario
        /// <summary>
        /// Acepta una solicitud de unión a un grupo.
        /// </summary>
        /// <param name="idSolicitud">ID de la solicitud a aceptar.</param>
        /// <returns>Indica si la aceptación fue exitosa.</returns>
        [HttpPost("aceptar/{idSolicitud}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AceptarSolicitudDeUnionGrupo(int idSolicitud, CancellationToken cancellationToken)
        {
            var resultado = await _servicioGrupo.AceptarSolicitudIngresoAGrupo(idSolicitud, cancellationToken);

            if (resultado.TieneErrores)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Error al aceptar la solicitud",
                    Detail = "Ah ocurrido un error cuando se intento aceptar la solicitud",
                    Status = StatusCodes.Status400BadRequest,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });
            }

            return NoContent();
        }

        // Crear un nuevo usuario
        /// <summary>
        /// Se une a un grupo utilizando un código.
        /// </summary>
        /// <param name="solicitud">Objeto que contiene la información para unirse al grupo.</param>
        /// <returns>Indica si la unión al grupo fue exitosa.</returns>
        [HttpPost("porCodigo")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UnirseConCodigoAGrupo([FromBody] UnirseAGrupoDTO solicitud, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Error de validación",
                    Detail = "El cuerpo de la solicitud es inválido, contiene errores de validación.",
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = ModelState.Keys
                        .SelectMany(key => ModelState[key].Errors.Select(error => new
                        {
                            Code = key,
                            Description = error.ErrorMessage
                        })) }
                });
            }

            var resultado = await _servicioGrupo.IngresoAGrupoConCodigo(solicitud, cancellationToken);

            if (resultado.TieneErrores)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Error al unirse al grupo por medio del código",
                    Detail = "Ah ocurrido un error al intentar unirse al grupo por medio del código",
                    Status = StatusCodes.Status400BadRequest,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });
            }

            return NoContent();
        }
    }
}
