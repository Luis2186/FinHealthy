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

            if (resultado.TieneErrores) return NotFound(resultado.Errores);

            return Ok(resultado);
        }

        [HttpGet("recibidas")]
        public async Task<ActionResult<Usuario>> ObtenerNotificacionesRecibidas([FromBody] string usuarioReceptorId)
        {
            var resultado = await _servicioNotificacion.ObtenerNotificacionesRecibidas(usuarioReceptorId);

            if (resultado.TieneErrores) return NotFound(resultado.Errores);

            return Ok(resultado);
        }

        [HttpPost("leerNotificacion")]
        public async Task<ActionResult<Usuario>> MarcarNotificacionLeida([FromBody] int notificacionId)
        {
            var resultado = await _servicioNotificacion.MarcarComoLeida(notificacionId);

            if (resultado.TieneErrores)
            {
                return NotFound(resultado.Errores);
            }

            return NoContent();
        }
        // Crear un nuevo usuario
        [HttpPost("enviarNotificacion")]
        public async Task<ActionResult<Usuario>> EnviarNotificacion([FromBody] NotificacionCreacionDTO notificacionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notificacionCreada = await _servicioNotificacion.EnviarNotificacion(notificacionDTO);
            
            // En caso de que el usuario ya exista o haya un error, devolver BadRequest
            if (notificacionCreada.TieneErrores)
            {
                return Conflict(notificacionCreada.Errores);
            }

            return Ok(notificacionCreada.Valor) ;
        }
        // Obtener un usuario por su ID
        [HttpGet("buscar")]
        public async Task<ActionResult<Usuario>> BuscarNotificacion([FromBody] int notificacionId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notificacion = await _servicioNotificacion.BuscarNotificacion(notificacionId);

            if (notificacion.TieneErrores)
            {
                return NotFound(notificacion.Errores);
            }

            return Ok(notificacion);  // Devuelve el usuario con estado 200 OK
        }
        // Actualizar un usuario
        [HttpDelete("eliminar")]
        public async Task<ActionResult<Usuario>> eliminarNotificacion([FromBody] int notificacionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notificacionEliminada = await _servicioNotificacion.EliminarNotificacion(notificacionId);

            if (notificacionEliminada.TieneErrores)
            {
                return NotFound(notificacionEliminada.Errores);
            }

            return NoContent(); 
        }


    }
}
