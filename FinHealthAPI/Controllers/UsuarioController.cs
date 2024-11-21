using AutoMapper;
using Dominio.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Servicio.Usuarios;
using Servicio.Usuarios.UsuariosDTO;

namespace FinHealthAPI.Controllers
{
    [Authorize(Roles = "Sys_Adm , Administrador")]
    [ApiController]
    [Route("/usuario")]
    public class UsuarioController : Controller
    {
        private readonly IServicioUsuario _servicioUsuario;
   
        private readonly IMapper _mapper;
        public UsuarioController(IServicioUsuario servicioUsuario, IMapper mapper)
        {
            _servicioUsuario = servicioUsuario;
            _mapper = mapper;
        }


        // Obtener todos los usuarios con paginación
        [HttpGet("paginados")]
        public async Task<ActionResult<Usuario>> ObtenerUsuariosPaginados()
        {
            var resultado = await _servicioUsuario.ObtenerTodos();
            return Ok(resultado);  // Devuelve los datos con estado HTTP 200 OK
        }

        // Obtener un usuario por su ID
        [HttpGet("obtener/{usuarioId}")]
        public async Task<ActionResult<Usuario>> ObtenerUsuarioPorId(string usuarioId)
        {
            var usuario = await _servicioUsuario.ObtenerPorId(usuarioId);
            if (usuario == null)
            {
                return NotFound();  // Devuelve 404 si no se encuentra el usuario
            }
            return Ok(usuario);  // Devuelve el usuario con estado 200 OK
        }

        // Crear un nuevo usuario
        [HttpPost("registrar")]
        [AllowAnonymous]
        public async Task<ActionResult<Usuario>> RegistrarUsuario([FromBody] CrearUsuarioDTO usuarioDto)
        {
            if (usuarioDto == null)
            {
                return BadRequest("Los datos del usuario son requeridos.");
            }

            var usuarioCreado = await _servicioUsuario.Registrar(usuarioDto);
            
            // En caso de que el usuario ya exista o haya un error, devolver BadRequest
            if (usuarioCreado == null)
            {
                return Conflict("Ya existe un usuario con este correo o nombre de usuario.");
            }

            return Ok(new { usuarioId = usuarioCreado.Id, usuarioCreado.Token }) ;
        }
        // Obtener un usuario por su ID
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<Usuario>> Login([FromBody] UsuarioLoginDTO usuarioDto)
        {
            var usuario = await _servicioUsuario.Login(usuarioDto);

            if (usuario == null)
            {
                return NotFound("Ah ocurrido un error al intentar loguearse, por favor verifique sus credenciales");  // Devuelve 404 si no se encuentra el usuario
            }

            //var token = _provedorJwt.Crear(usuario);

            return Ok( new { usuarioId = usuario.Id , usuario.Token });  // Devuelve el usuario con estado 200 OK
        }
        // Actualizar un usuario
        [HttpPut("actualizar/{usuarioId}")]
        public async Task<ActionResult<Usuario>> ActualizarUsuario(string usuarioId, [FromBody] ActualizarUsuarioDTO usuarioDto)
        {
            if (usuarioDto == null)
            {
                return BadRequest("Los datos del usuario son requeridos.");
            }

            var usuarioActualizado = await _servicioUsuario.Actualizar(usuarioId, usuarioDto);

            if (usuarioActualizado == null)
            {
                return NotFound($"Usuario con id {usuarioId} no encontrado.");
            }

            return Ok(usuarioActualizado);  // Devuelve el usuario actualizado con estado 200 OK
        }

        // Eliminar un usuario
        [HttpDelete("eliminar/{usuarioId}")]
        public async Task<ActionResult> EliminarUsuario(string usuarioId)
        {
            var exito = await _servicioUsuario.Eliminar(usuarioId);

            if (!exito)
            {
                return NotFound($"Usuario con id {usuarioId} no encontrado.");
            }

            return NoContent();  // Devuelve 204 No Content si la eliminación fue exitosa
        }
        [HttpDelete("eliminarRol/{usuarioId}")]
        public async Task<ActionResult> EliminarRolAUsuario(string usuarioId, [FromBody] UsuarioRolDTO rol)
        {
            var exito = await _servicioUsuario.RemoverRol(usuarioId,rol.IdRol);

            if (!exito)
            {
                return NotFound($"El rol con id {rol.IdRol} no se pudo eliminar.");
            }

            return NoContent();  // Devuelve 204 No Content si la eliminación fue exitosa
        }

        [HttpPost("agregarRol/{usuarioId}")]
        public async Task<ActionResult> AgregarRolAUsuario(string usuarioId, [FromBody] UsuarioRolDTO rol)
        {
            var exito = await _servicioUsuario.AgregarRol(usuarioId, rol.IdRol);

            if (!exito)
            {
                return NotFound($"El rol con id {rol.IdRol} no se pudo agregar al usuario con id {rol.IdRol}.");
            }

            return NoContent();  // Devuelve 204 No Content si la eliminación fue exitosa
        }

        // Obtener todos los usuarios con paginación
        [HttpGet("obtenerRoles/{usuarioId}")]
        public async Task<ActionResult<Usuario>> ObtenerRolesPorUsuario(string usuarioId)
        {
            var resultado = await _servicioUsuario.ObtenerRolesPorUsuario(usuarioId);
            return Ok(resultado);  // Devuelve los datos con estado HTTP 200 OK
        }


    }
}
