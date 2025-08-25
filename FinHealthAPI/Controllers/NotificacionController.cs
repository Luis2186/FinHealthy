using AutoMapper;
using Dominio;
using Dominio.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Servicio.DTOS.NotificacionesDTO;
using Servicio.Notificaciones;
using Servicio.Usuarios;

namespace FinHealthAPI.Controllers
{
    /// <summary>
    /// Controlador para la gestión de notificaciones.
    /// </summary>
    [Authorize(Roles = "Sys_Adm , Administrador, Usuario")]
    [ApiController]
    [Route("/notificacion")]
    public class NotificacionController : Controller
    {
        private readonly IServicioNotificacion _servicioNotificacion;
   
        public NotificacionController(IServicioNotificacion servicioNotificacion, IMapper mapper)
        {
            _servicioNotificacion = servicioNotificacion;
        }

        /// <summary>
        /// Obtiene las notificaciones emitidas por un usuario.
        /// </summary>
        /// <param name="usuarioEmisorId">El ID del usuario emisor.</param>
        /// <returns>Una lista de notificaciones emitidas.</returns>
        [HttpGet("emitidas")]
        [ProducesResponseType(typeof(IEnumerable<NotificacionDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<NotificacionDTO>>> ObtenerNotificacionesEmitidas([FromBody] string usuarioEmisorId, CancellationToken cancellationToken)
        {
            var resultado = await _servicioNotificacion.ObtenerNotificacionesEmitidas(usuarioEmisorId, cancellationToken);

            if (resultado.TieneErrores)
                return NotFound(new ProblemDetails
                {
                    Title = "Error al obtener notificaciones emitidas",
                    Detail = resultado.ObtenerErroresComoString(),
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });

            return Ok(resultado.Valor);
        }

        /// <summary>
        /// Obtiene las notificaciones recibidas por un usuario.
        /// </summary>
        /// <param name="usuarioReceptorId">El ID del usuario receptor.</param>
        /// <returns>Una lista de notificaciones recibidas.</returns>
        [HttpGet("recibidas")]
        [ProducesResponseType(typeof(IEnumerable<NotificacionDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<NotificacionDTO>>> ObtenerNotificacionesRecibidas([FromBody] string usuarioReceptorId, CancellationToken cancellationToken)
        {
            var resultado = await _servicioNotificacion.ObtenerNotificacionesRecibidas(usuarioReceptorId, cancellationToken);

            if (resultado.TieneErrores)
                return NotFound(new ProblemDetails
                {
                    Title = "Error al obtener notificaciones recibidas",
                    Detail = resultado.ObtenerErroresComoString(),
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });

            return Ok(resultado.Valor);
        }

        /// <summary>
        /// Marca una notificación como leída.
        /// </summary>
        /// <param name="notificacionId">El ID de la notificación.</param>
        /// <returns>Un resultado vacío si la operación fue exitosa.</returns>
        [HttpPost("leerNotificacion")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> MarcarNotificacionLeida([FromBody] int notificacionId, CancellationToken cancellationToken)
        {
            var resultado = await _servicioNotificacion.MarcarComoLeida(notificacionId, cancellationToken);

            if (resultado.TieneErrores)
                return NotFound(new ProblemDetails
                {
                    Title = "Error al marcar notificación como leída",
                    Detail = resultado.ObtenerErroresComoString(),
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });

            return NoContent();
        }

        /// <summary>
        /// Envía una nueva notificación.
        /// </summary>
        /// <param name="notificacionDTO">El objeto de notificación a enviar.</param>
        /// <returns>Los detalles de la notificación enviada.</returns>
        [HttpPost("enviarNotificacion")]
        [ProducesResponseType(typeof(NotificacionDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<NotificacionDTO>> EnviarNotificacion([FromBody] CrearNotificacionDTO notificacionDTO, CancellationToken cancellationToken)
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

            var resultado = await _servicioNotificacion.EnviarNotificacion(notificacionDTO, cancellationToken);

            if (resultado.TieneErrores)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Error al enviar notificación",
                    Detail = "Ah ocurrido un error al intentar enviar la notificación",
                    Status = StatusCodes.Status400BadRequest,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });
            }

            return CreatedAtAction(nameof(EnviarNotificacion), new { id = resultado.Valor.Id }, resultado.Valor);
        }

        /// <summary>
        /// Busca una notificación por su ID.
        /// </summary>
        /// <param name="notificacionId">El ID de la notificación.</param>
        /// <returns>Los detalles de la notificación buscada.</returns>
        [HttpGet("buscar")]
        [ProducesResponseType(typeof(NotificacionDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<NotificacionDTO>> BuscarNotificacion([FromBody] int notificacionId, CancellationToken cancellationToken)
        {
            var resultado = await _servicioNotificacion.BuscarNotificacion(notificacionId, cancellationToken);

            if (resultado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error al buscar la notificación por id",
                    Detail = "Ah ocurrido un error al intentar buscar la notificación por id",
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });
            }

            return Ok(resultado.Valor);
        }

        /// <summary>
        /// Elimina una notificación.
        /// </summary>
        /// <param name="notificacionId">El ID de la notificación a eliminar.</param>
        /// <returns>Un resultado vacío si la operación fue exitosa.</returns>
        [HttpDelete("eliminar")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> EliminarNotificacion([FromBody] int notificacionId, CancellationToken cancellationToken)
        {
            var resultado = await _servicioNotificacion.EliminarNotificacion(notificacionId, cancellationToken);

            if (resultado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error al eliminar la notificación",
                    Detail = "Ah ocurrido un error al intentar eliminar la notificación",
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });
            }

            return NoContent();
        }


    }
}
