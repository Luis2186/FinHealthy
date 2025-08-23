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
        public async Task<ActionResult<Usuario>> ObtenerNotificacionesEmitidas([FromBody] string usuarioEmisorId)
        {
            var resultado = await _servicioNotificacion.ObtenerNotificacionesEmitidas(usuarioEmisorId, HttpContext.RequestAborted);

            if (resultado.TieneErrores) return NotFound(new ProblemDetails
            {
                Title = "Error al eliminar categoria",
                Detail = resultado.ObtenerErroresComoString(),
                Status = 404,
                Instance = HttpContext.Request.Path,
                Extensions = {
                        ["errors"] = resultado.Errores
                    }
            });

            return Ok(resultado);
        }

        /// <summary>
        /// Obtiene las notificaciones recibidas por un usuario.
        /// </summary>
        /// <param name="usuarioReceptorId">El ID del usuario receptor.</param>
        /// <returns>Una lista de notificaciones recibidas.</returns>
        [HttpGet("recibidas")]
        public async Task<ActionResult<Usuario>> ObtenerNotificacionesRecibidas([FromBody] string usuarioReceptorId)
        {
            var resultado = await _servicioNotificacion.ObtenerNotificacionesRecibidas(usuarioReceptorId, HttpContext.RequestAborted);

            if (resultado.TieneErrores) return NotFound(new ProblemDetails
            {
                Title = "Error al eliminar categoria",
                Detail = resultado.ObtenerErroresComoString(),
                Status = 404,
                Instance = HttpContext.Request.Path,
                Extensions = {
                        ["errors"] = resultado.Errores
                    }
            });

            return Ok(resultado);
        }

        /// <summary>
        /// Marca una notificación como leída.
        /// </summary>
        /// <param name="notificacionId">El ID de la notificación.</param>
        /// <returns>Un resultado vacío si la operación fue exitosa.</returns>
        [HttpPost("leerNotificacion")]
        public async Task<ActionResult<Usuario>> MarcarNotificacionLeida([FromBody] int notificacionId)
        {
            var resultado = await _servicioNotificacion.MarcarComoLeida(notificacionId, HttpContext.RequestAborted);

            if (resultado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error al eliminar categoria",
                    Detail = resultado.ObtenerErroresComoString(),
                    Status = 404,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = resultado.Errores
                    }
                });
            }

            return NoContent();
        }

        /// <summary>
        /// Envía una nueva notificación.
        /// </summary>
        /// <param name="notificacionDTO">El objeto de notificación a enviar.</param>
        /// <returns>Los detalles de la notificación enviada.</returns>
        [HttpPost("enviarNotificacion")]
        public async Task<ActionResult<Usuario>> EnviarNotificacion([FromBody] CrearNotificacionDTO notificacionDTO)
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
                                Code = key,
                                Description = error.ErrorMessage
                            }))
                    }
                });
            }

            var resultado = await _servicioNotificacion.EnviarNotificacion(notificacionDTO, HttpContext.RequestAborted);
            
            // En caso de que el usuario ya exista o haya un error, devolver BadRequest
            if (resultado.TieneErrores)
            {
                return Conflict(new ProblemDetails
                {
                    Title = "Error al enviar notificacion",
                    Detail = "Ah ocurrido un error al intentar enviar la notificacion",
                    Status = 404,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = resultado.Errores
                    }
                });
            }

            return Ok(resultado.Valor) ;
        }

        /// <summary>
        /// Busca una notificación por su ID.
        /// </summary>
        /// <param name="notificacionId">El ID de la notificación.</param>
        /// <returns>Los detalles de la notificación buscada.</returns>
        [HttpGet("buscar")]
        public async Task<ActionResult<Usuario>> BuscarNotificacion([FromBody] int notificacionId)
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

            var resultado = await _servicioNotificacion.BuscarNotificacion(notificacionId, HttpContext.RequestAborted);

            if (resultado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error al buscar la notificacion por id",
                    Detail = "Ah ocurrido un error al intentar buscar la notificacion por id",
                    Status = 404,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = resultado.Errores
                    }
                });
            }

            return Ok(resultado);  // Devuelve el usuario con estado 200 OK
        }

        /// <summary>
        /// Elimina una notificación.
        /// </summary>
        /// <param name="notificacionId">El ID de la notificación a eliminar.</param>
        /// <returns>Un resultado vacío si la operación fue exitosa.</returns>
        [HttpDelete("eliminar")]
        public async Task<ActionResult<Usuario>> eliminarNotificacion([FromBody] int notificacionId)
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

            var resultado = await _servicioNotificacion.EliminarNotificacion(notificacionId, HttpContext.RequestAborted);

            if (resultado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error al eliminar la notificacion",
                    Detail = "Ah ocurrido un error al intentar eliminar la notificacion",
                    Status = 404,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = resultado.Errores
                    }
                });
            }

            return NoContent(); 
        }


    }
}
