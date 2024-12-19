using AutoMapper;
using Dominio;
using Dominio.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Servicio.Notificaciones;
using Servicio.Notificaciones.NotificacionesDTO;
using Servicio.Usuarios;

namespace FinHealthAPI.Controllers
{
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

        [HttpGet("emitidas")]
        public async Task<ActionResult<Usuario>> ObtenerNotificacionesEmitidas([FromBody] string usuarioEmisorId)
        {
            var resultado = await _servicioNotificacion.ObtenerNotificacionesEmitidas(usuarioEmisorId);

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

        [HttpGet("recibidas")]
        public async Task<ActionResult<Usuario>> ObtenerNotificacionesRecibidas([FromBody] string usuarioReceptorId)
        {
            var resultado = await _servicioNotificacion.ObtenerNotificacionesRecibidas(usuarioReceptorId);

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

        [HttpPost("leerNotificacion")]
        public async Task<ActionResult<Usuario>> MarcarNotificacionLeida([FromBody] int notificacionId)
        {
            var resultado = await _servicioNotificacion.MarcarComoLeida(notificacionId);

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
        // Crear un nuevo usuario
        [HttpPost("enviarNotificacion")]
        public async Task<ActionResult<Usuario>> EnviarNotificacion([FromBody] NotificacionCreacionDTO notificacionDTO)
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
                        ["errors"] = ModelState.Keys.ToDictionary(
                            key => key,
                            key => ModelState[key].Errors.Select(e => e.ErrorMessage).ToArray())
                    }
                });
            }

            var resultado = await _servicioNotificacion.EnviarNotificacion(notificacionDTO);
            
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
        // Obtener un usuario por su ID
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
                        ["errors"] = ModelState.Keys.ToDictionary(
                            key => key,
                            key => ModelState[key].Errors.Select(e => e.ErrorMessage).ToArray())
                    }
                });
            }

            var resultado = await _servicioNotificacion.BuscarNotificacion(notificacionId);

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
        // Actualizar un usuario
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
                        ["errors"] = ModelState.Keys.ToDictionary(
                            key => key,
                            key => ModelState[key].Errors.Select(e => e.ErrorMessage).ToArray())
                    }
                });
            }

            var resultado = await _servicioNotificacion.EliminarNotificacion(notificacionId);

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
