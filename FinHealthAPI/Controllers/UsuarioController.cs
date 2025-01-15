using AutoMapper;
using Azure.Core;
using Dominio;
using Dominio.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using Servicio.DTOS.UsuariosDTO;
using Servicio.Pdf;
using Servicio.ServiciosExternos;
using Servicio.Usuarios;
using System.Configuration;

namespace FinHealthAPI.Controllers
{
    [Authorize(Roles = "Sys_Adm,Administrador")]
    [ApiController]
    [Route("/usuario")]
    public class UsuarioController : Controller
    {
        private readonly IServicioUsuario _servicioUsuario;
        private readonly IServicioMonedas _servicioMoneda;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public UsuarioController(IServicioUsuario servicioUsuario, IMapper mapper,
            IServicioMonedas servicioMoneda, IConfiguration config)
        {
            _servicioUsuario = servicioUsuario;
            _servicioMoneda = servicioMoneda;
            _mapper = mapper;
            _config = config;
        }


        // Obtener todos los usuarios con paginación
        [HttpGet("paginados")]
        public async Task<ActionResult<Usuario>> ObtenerUsuariosPaginados()
        {
            var resultado = await _servicioUsuario.ObtenerTodos();

            if (resultado.TieneErrores) return NotFound(new ProblemDetails
            {
                Title = "Error al obtener usuarios paginados",
                Detail = "Ah ocurrido un error cuando se intento obtener los usuarios",
                Status = 404,
                Instance = HttpContext.Request.Path,
                Extensions = {
                        ["errors"] = resultado.Errores
                }
            });

            var usuariosDTOS = _mapper.Map<List<UsuarioDTO>>(resultado.Valor);
            
            var usuariosPDFDTOS = _mapper.Map<List<UsuarioPDFDTO>>(resultado.Valor);

            var reporte = new Pdf<UsuarioPDFDTO>(usuariosPDFDTOS, "Listado de Usuarios");
            reporte.GeneratePdf("C:\\Users\\lilp_\\Desktop\\Usuarios.pdf");

            return Ok(usuariosDTOS);  // Devuelve los datos con estado HTTP 200 OK
        }

        // Obtener un usuario por su ID
        [HttpGet("obtener/{usuarioId}")]
        [AllowAnonymous]
        public async Task<ActionResult<Usuario>> ObtenerUsuarioPorId(string usuarioId)
        {
            var usuario = await _servicioUsuario.ObtenerPorId(usuarioId);
            
            if (usuario.TieneErrores) return NotFound(new ProblemDetails
            {
                Title = "Error al obtener usuario por id",
                Detail = "Ah ocurrido un error al intentar obtener el usuario por id",
                Status = 404,
                Instance = HttpContext.Request.Path,
                Extensions = {
                        ["errors"] = usuario.Errores
                }
            }); 

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

            var usuarioCreado = await _servicioUsuario.Registrar(usuarioDto);
            
            // En caso de que el usuario ya exista o haya un error, devolver BadRequest
            if (usuarioCreado.TieneErrores)
            {
                return Conflict(new ProblemDetails
                {
                    Title = "Error al registrar usuario",
                    Detail = "Ah ocurrido un error cuando se intento registrar el usuario",
                    Status = 409,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = usuarioCreado.Errores
                    }
                });
            }

            var (accessToken, refreshToken, usuarioId) = usuarioCreado.Valor;
            AgregarTokensACookies(accessToken, refreshToken);

            return Ok(new { mensaje = "Registro exitoso", id = usuarioId }) ;
        }
        // Obtener un usuario por su ID
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<Usuario>> Login([FromBody] UsuarioLoginDTO usuarioDto)
        {
            //await _servicioMoneda.ActualizarMonedasDesdeServicio();

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

            var usuario = await _servicioUsuario.Login(usuarioDto);

            if (usuario.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error Login",
                    Detail = "Ah ocurrido un error al intentar loguearse",
                    Status = 404,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = usuario.Errores
                    }
                });
            }
            // Generar el token JWT
            var (accessToken, refreshToken, usuarioId) = usuario.Valor;
            AgregarTokensACookies(accessToken, refreshToken);

            return Ok( new { mensaje = "Inicio de sesión exitoso", id = usuarioId });  // Devuelve el usuario con estado 200 OK
        }

        // Obtener un usuario por su ID
        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<ActionResult<Usuario>> Logout()
        {
            Response.Cookies.Append("token", "", new CookieOptions
            {
                HttpOnly = true,   // No accesible desde JavaScript
                Secure = true,     // Solo se enviará a través de HTTPS
                SameSite = SameSiteMode.None, // Asegura que no se envíe en solicitudes de terceros
                Expires = DateTime.Now.AddDays(-1), // Expira en 1 día para eliminarla
                Path = "/"         // El dominio de la cookie debe coincidir con el path original
            });

            return Ok("Cookie eliminada correctamente");
        }


        // Actualizar un usuario
        [HttpPut("actualizar/{usuarioId}")]
        public async Task<ActionResult<Usuario>> ActualizarUsuario(string usuarioId, [FromBody] ActualizarUsuarioDTO usuarioDto)
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

            var usuarioActualizado = await _servicioUsuario.Actualizar(usuarioId, usuarioDto);

            if (usuarioActualizado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error al actualizar usuario",
                    Detail = "Ah ocurrido un error al actualizar usuario",
                    Status = 404,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = usuarioActualizado.Errores
                    }
                });
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
                return NotFound(new ProblemDetails
                {
                    Title = "Error al eliminar usuario",
                    Detail = usuarioEliminado.ObtenerErroresComoString(),
                    Status = 404,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = usuarioEliminado.Errores
                    }
                });
            }

            return NoContent();  // Devuelve 204 No Content si la eliminación fue exitosa
        }
        [HttpDelete("eliminarRol")]
        public async Task<ActionResult> EliminarRolAUsuario([FromBody] UsuarioRolDTO rol)
        {
            var rolEliminado = await _servicioUsuario.RemoverRol(rol.idUsuario, rol.IdRol,rol.NombreRol);

            if (rolEliminado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error al eliminar rol a usuario",
                    Detail = "Ah ocurrido un error al intentar eliminar el rol del usuario",
                    Status = 404,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = rolEliminado.Errores
                    }
                });
            }

            return NoContent();  // Devuelve 204 No Content si la eliminación fue exitosa
        }

        [HttpPost("agregarRol")]
        public async Task<ActionResult> AgregarRolAUsuario([FromBody] UsuarioRolDTO rol)
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

            var rolAgregado = await _servicioUsuario.AgregarRol(rol.idUsuario, rol.IdRol,rol.NombreRol);

            if (rolAgregado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error al intentar agregar rol a usuario",
                    Detail = "Ah ocurrido un error al intentar agregar el rol al usuario",
                    Status = 404,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = rolAgregado.Errores
                    }
                });
            }

            return NoContent();  // Devuelve 204 No Content si la eliminación fue exitosa
        }

        // Obtener todos los usuarios con paginación
        [HttpGet("obtenerRoles/{usuarioId}")]
        public async Task<ActionResult<Usuario>> ObtenerRolesPorUsuario(string usuarioId)
        {
            var resultado = await _servicioUsuario.ObtenerRolesPorUsuario(usuarioId);
            
            if(resultado.TieneErrores) return NotFound(new ProblemDetails
            {
                Title = "Error roles de usuario",
                Detail = "Ah ocurrido un error al intentar obtener los roles del usuario",
                Status = 404,
                Instance = HttpContext.Request.Path,
                Extensions = {
                        ["errors"] = resultado.Errores
                    }
            });

            return Ok(resultado.Valor);  // Devuelve los datos con estado HTTP 200 OK
        }

        private void AgregarTokensACookies(string accessToken, string refreshToken)
        {
            var expiracionToken = _config.GetValue<int>("Jwt:ExpiracionAccessToken"); 
            var expiracionRefreshToken = _config.GetValue<int>("Jwt:ExpiracionRefreshToken");
            var accessTokenCookieName = _config.GetValue<string>("Jwt:AccessTokenCookieName"); 
            var refreshTokenCookieName = _config.GetValue<string>("Jwt:refreshTokenCookieName");

            // Crear una cookie segura con HttpOnly y Secure activados, para tener mas seguridad.
            Response.Cookies.Append(accessTokenCookieName, accessToken, new CookieOptions
            {
                HttpOnly = true,  // No accesible desde el cliente
                Secure = true,    // Solo se enviará a través de HTTPS
                SameSite = SameSiteMode.None, // Evitar que se envíe en solicitudes de terceros
                Expires = DateTime.Now.AddMinutes(expiracionToken), 
                Path = "/"
            });

            Response.Cookies.Append(refreshTokenCookieName, refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(expiracionRefreshToken)
            });
        }

    }
}
