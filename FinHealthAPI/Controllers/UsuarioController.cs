using Dominio.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Servicio.Usuarios;
using Servicio.Usuarios.UsuariosDTO;

namespace FinHealthAPI.Controllers
{
    [ApiController]
    [Route("/usuario")]
    public class UsuarioController : Controller
    {
        private readonly IServicioUsuario _servicioUsuario;

        public UsuarioController(IServicioUsuario servicioUsuario)
        {
            _servicioUsuario = servicioUsuario; 
        }


        // Obtener todos los usuarios con paginación
        [HttpGet("paginados")]
        public async Task<ActionResult<Usuario>> ObtenerUsuariosPaginados()
        {
            var resultado = await _servicioUsuario.ObtenerTodos();
            return Ok(resultado);  // Devuelve los datos con estado HTTP 200 OK
        }

        // Obtener un usuario por su ID
        [HttpGet("obtener/{id}")]
        public async Task<ActionResult<Usuario>> ObtenerUsuarioPorId(string id)
        {
            var usuario = await _servicioUsuario.ObtenerPorId(id);
            if (usuario == null)
            {
                return NotFound();  // Devuelve 404 si no se encuentra el usuario
            }
            return Ok(usuario);  // Devuelve el usuario con estado 200 OK
        }

        // Crear un nuevo usuario
        [HttpPost("crear")]
        public async Task<ActionResult<Usuario>> CrearUsuario([FromBody] CrearUsuarioDTO usuarioDto)
        {
            if (usuarioDto == null)
            {
                return BadRequest("Los datos del usuario son requeridos.");
            }

            var usuarioCreado = await _servicioUsuario.Crear(usuarioDto);

            // En caso de que el usuario ya exista o haya un error, devolver BadRequest
            if (usuarioCreado == null)
            {
                return Conflict("Ya existe un usuario con este correo o nombre de usuario.");
            }

            return CreatedAtAction(nameof(ObtenerUsuarioPorId), new { id = usuarioCreado.Id }, usuarioCreado);
        }

        // Actualizar un usuario
        [HttpPut("actualizar/{id}")]
        public async Task<ActionResult<Usuario>> ActualizarUsuario(string id, [FromBody] ActualizarUsuarioDTO usuarioDto)
        {
            if (usuarioDto == null)
            {
                return BadRequest("Los datos del usuario son requeridos.");
            }

            var usuarioActualizado = await _servicioUsuario.Actualizar(id,usuarioDto);

            if (usuarioActualizado == null)
            {
                return NotFound($"Usuario con id {id} no encontrado.");
            }

            return Ok(usuarioActualizado);  // Devuelve el usuario actualizado con estado 200 OK
        }

        // Eliminar un usuario
        [HttpDelete("eliminar/{id}")]
        public async Task<ActionResult> EliminarUsuario(string id)
        {
            var exito = await _servicioUsuario.Eliminar(id);

            if (!exito)
            {
                return NotFound($"Usuario con id {id} no encontrado.");
            }

            return NoContent();  // Devuelve 204 No Content si la eliminación fue exitosa
        }
    }
}
