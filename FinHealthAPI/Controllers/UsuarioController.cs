using AutoMapper;
using Dominio;
using Dominio.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using Servicio.Pdf;
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

            if (resultado.TieneErrores) return NotFound(resultado.Errores);

            var usuariosDTOS = _mapper.Map<List<UsuarioDTO>>(resultado.Valor);
            
            var usuariosPDFDTOS = _mapper.Map<List<UsuarioPDFDTO>>(resultado.Valor);

            var reporte = new Pdf<UsuarioPDFDTO>(usuariosPDFDTOS, "Listado de Usuarios");
            reporte.GeneratePdf("C:\\Users\\lilp_\\Desktop\\Usuarios.pdf");

            return Ok(usuariosDTOS);  // Devuelve los datos con estado HTTP 200 OK
        }

        // Obtener un usuario por su ID
        [HttpGet("obtener/{usuarioId}")]
        public async Task<ActionResult<Usuario>> ObtenerUsuarioPorId(string usuarioId)
        {
            var usuario = await _servicioUsuario.ObtenerPorId(usuarioId);
            if (usuario.TieneErrores)
            {
                return NotFound(usuario.Errores);  // Devuelve 404 si no se encuentra el usuario
            }

            var usuarioDTO = _mapper.Map<UsuarioDTO>(usuario.Valor);

            return Ok(usuarioDTO);  // Devuelve el usuario con estado 200 OK
        }

        // Crear un nuevo usuario
        [HttpPost("registrar")]
        [AllowAnonymous]
        public async Task<ActionResult<Usuario>> RegistrarUsuario([FromBody] CrearUsuarioDTO usuarioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuarioCreado = await _servicioUsuario.Registrar(usuarioDto);
            
            // En caso de que el usuario ya exista o haya un error, devolver BadRequest
            if (usuarioCreado.TieneErrores)
            {
                return Conflict(usuarioCreado.Errores);
            }

            return Ok(new { usuarioId = usuarioCreado.Valor.Id, usuarioCreado.Valor.Token }) ;
        }
        // Obtener un usuario por su ID
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<Usuario>> Login([FromBody] UsuarioLoginDTO usuarioDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = await _servicioUsuario.Login(usuarioDto);

            if (usuario.TieneErrores)
            {
                return NotFound(usuario.Errores);
            }

            //var token = _provedorJwt.Crear(usuario);

            return Ok( new { usuarioId = usuario.Valor.Id , usuario.Valor.Token });  // Devuelve el usuario con estado 200 OK
        }
        // Actualizar un usuario
        [HttpPut("actualizar/{usuarioId}")]
        public async Task<ActionResult<Usuario>> ActualizarUsuario(string usuarioId, [FromBody] ActualizarUsuarioDTO usuarioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuarioActualizado = await _servicioUsuario.Actualizar(usuarioId, usuarioDto);

            if (usuarioActualizado.TieneErrores)
            {
                return NotFound(usuarioActualizado.Errores);
            }

            var usuarioDTO = _mapper.Map<UsuarioDTO>(usuarioActualizado.Valor);

            return Ok(usuarioDTO);  // Devuelve el usuario actualizado con estado 200 OK
        }

        // Eliminar un usuario
        [HttpDelete("eliminar/{usuarioId}")]
        public async Task<ActionResult> EliminarUsuario(string usuarioId)
        {
            var usuarioEliminado = await _servicioUsuario.Eliminar(usuarioId);

            if (usuarioEliminado.TieneErrores)
            {
                return NotFound(usuarioEliminado.Errores);
            }

            return NoContent();  // Devuelve 204 No Content si la eliminación fue exitosa
        }
        [HttpDelete("eliminarRol")]
        public async Task<ActionResult> EliminarRolAUsuario([FromBody] UsuarioRolDTO rol)
        {
            var rolEliminado = await _servicioUsuario.RemoverRol(rol.idUsuario, rol.IdRol,rol.NombreRol);

            if (rolEliminado.TieneErrores)
            {
                return NotFound(rolEliminado.Errores);
            }

            return NoContent();  // Devuelve 204 No Content si la eliminación fue exitosa
        }

        [HttpPost("agregarRol")]
        public async Task<ActionResult> AgregarRolAUsuario([FromBody] UsuarioRolDTO rol)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rolAgregado = await _servicioUsuario.AgregarRol(rol.idUsuario, rol.IdRol,rol.NombreRol);

            if (rolAgregado.TieneErrores)
            {
                return NotFound(rolAgregado.Errores);
            }

            return NoContent();  // Devuelve 204 No Content si la eliminación fue exitosa
        }

        // Obtener todos los usuarios con paginación
        [HttpGet("obtenerRoles/{usuarioId}")]
        public async Task<ActionResult<Usuario>> ObtenerRolesPorUsuario(string usuarioId)
        {
            var resultado = await _servicioUsuario.ObtenerRolesPorUsuario(usuarioId);
            
            if(resultado.TieneErrores) return NotFound(resultado.Errores);

            return Ok(resultado.Valor);  // Devuelve los datos con estado HTTP 200 OK
        }


    }
}
