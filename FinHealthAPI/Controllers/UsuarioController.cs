using AutoMapper;
using Dominio;
using Dominio.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Servicio.DTOS.UsuariosDTO;
using Servicio.Usuarios;
using Servicio.Usuarios.Authentication;
using Servicio.ServiciosExternos; // IServicioMonedas
using Servicio.S_Pdf; // Pdf<T>
using Servicio.DTOS.UsuariosDTO; // UsuarioPDFDTO
using System.Configuration;
using System.Net;

namespace FinHealthAPI.Controllers
{
    /// <summary>
    /// Controlador para la gestión de usuarios.
    /// </summary>
    [Authorize(Roles = "Sys_Adm,Administrador")]
    [ApiController]
    [Route("/usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly IServicioUsuario _servicioUsuario;
        private readonly IServicioMonedas _servicioMoneda;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly string refreshTokenCookieName;
        private readonly string accessTokenCookieName;
        public UsuarioController(IServicioUsuario servicioUsuario, IMapper mapper,
            IServicioMonedas servicioMoneda, IConfiguration config)
        {
            _servicioUsuario = servicioUsuario;
            _servicioMoneda = servicioMoneda;
            _mapper = mapper;
            _config = config;
            refreshTokenCookieName = _config.GetValue<string>("Jwt:refreshTokenCookieName");
            accessTokenCookieName  = _config.GetValue<string>("Jwt:AccessTokenCookieName");
        }


        /// <summary>
        /// Obtener todos los usuarios con paginación
        /// </summary>
        [HttpGet("paginados")]
        [ProducesResponseType(typeof(IEnumerable<UsuarioDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> ObtenerUsuariosPaginados(CancellationToken cancellationToken)
        {
            var resultado = await _servicioUsuario.ObtenerTodos(cancellationToken);

            if (resultado.TieneErrores)
                return NotFound(new ProblemDetails
                {
                    Title = "Error al obtener usuarios paginados",
                    Detail = "Ah ocurrido un error cuando se intento obtener los usuarios",
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });

            var usuariosDTOS = _mapper.Map<List<UsuarioDTO>>(resultado.Valor);
            return Ok(usuariosDTOS);
        }

        /// <summary>
        /// Obtener un usuario por su ID
        /// </summary>
        [HttpGet("obtener/{usuarioId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UsuarioDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UsuarioDTO>> ObtenerUsuarioPorId(string usuarioId, CancellationToken cancellationToken)
        {
            var usuario = await _servicioUsuario.ObtenerPorId(usuarioId, cancellationToken);

            if (usuario.TieneErrores)
                return NotFound(new ProblemDetails
                {
                    Title = "Error al obtener usuario por id",
                    Detail = "Ah ocurrido un error al intentar obtener el usuario por id",
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = usuario.Errores }
                });

            var usuarioDTO = _mapper.Map<UsuarioDTO>(usuario.Valor);
            return Ok(usuarioDTO);
        }

        /// <summary>
        /// Crear un nuevo usuario
        /// </summary>
        [HttpPost("registrar")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> RegistrarUsuario([FromBody] CrearUsuarioDTO usuarioDto, CancellationToken cancellationToken)
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

            var usuarioCreado = await _servicioUsuario.Registrar(usuarioDto, cancellationToken);

            if (usuarioCreado.TieneErrores)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Error al registrar usuario",
                    Detail = "Ah ocurrido un error cuando se intento registrar el usuario",
                    Status = StatusCodes.Status400BadRequest,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = usuarioCreado.Errores }
                });
            }

            (string accessToken, string refreshToken, string usuarioId) = usuarioCreado.Valor;
            AgregarTokensACookies(accessToken, refreshToken);
            return CreatedAtAction(nameof(RegistrarUsuario), new { id = usuarioId }, new { mensaje = "Registro exitoso", id = usuarioId });
        }
        /// <summary>
        /// Obtener un usuario por su ID
        /// </summary>
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

            var usuario = await _servicioUsuario.Login(usuarioDto, HttpContext.RequestAborted);

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
            (string accessToken, string refreshToken, string usuarioId) = usuario.Valor;
            AgregarTokensACookies(accessToken, refreshToken);

            return Ok( new { mensaje = "Inicio de sesión exitoso", id = usuarioId });  // Devuelve el usuario con estado 200 OK
        }

        /// <summary>
        /// Obtener un usuario por su ID
        /// </summary>
        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<ActionResult<Usuario>> Logout()
        {
            //var accessTokenCookieName = _config.GetValue<string>("Jwt:AccessTokenCookieName");
            //var refreshTokenCookieName = _config.GetValue<string>("Jwt:refreshTokenCookieName");

            if (refreshTokenCookieName != null && refreshTokenCookieName != "") {
                Request.Cookies.TryGetValue(refreshTokenCookieName, out string refreshTokenCookie);

                if (refreshTokenCookie != null && refreshTokenCookie != "") await _servicioUsuario.RevocarRefreshToken(refreshTokenCookie, HttpContext.RequestAborted);

            } 

            LimpiarCookies();

            return Ok("Cookie eliminada correctamente");
        }


        /// <summary>
        /// Actualizar un usuario
        /// </summary>
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

            var usuarioActualizado = await _servicioUsuario.Actualizar(usuarioId, usuarioDto, HttpContext.RequestAborted);

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

        /// <summary>
        /// Eliminar un usuario
        /// </summary>
        [HttpDelete("eliminar/{usuarioId}")]
        public async Task<ActionResult> EliminarUsuario(string usuarioId)
        {
            var usuarioEliminado = await _servicioUsuario.Eliminar(usuarioId, HttpContext.RequestAborted);

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
        /// <summary>
        /// Eliminar un rol a un usuario
        /// </summary>
        [HttpDelete("eliminarRol/{idUsuario}/{idRol}/{nombreRol}")]
        public async Task<ActionResult> EliminarRolAUsuario(string idUsuario,string nombreRol, string idRol)
        {
            var rolEliminado = await _servicioUsuario.RemoverRol(idUsuario, idRol, nombreRol, HttpContext.RequestAborted);

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

        /// <summary>
        /// Agregar un rol a un usuario
        /// </summary>
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
                                Code = key,
                                Description = error.ErrorMessage
                            }))
                    }
                });
            }

            var rolAgregado = await _servicioUsuario.AgregarRol(rol.idUsuario, rol.IdRol, rol.NombreRol, HttpContext.RequestAborted);

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

            return NoContent();
        }

        /// <summary>
        /// Obtener todos los roles de un usuario
        /// </summary>
        [HttpGet("obtenerRoles/{usuarioId}")]
        public async Task<ActionResult<Usuario>> ObtenerRolesPorUsuario(string usuarioId)
        {
            var resultado = await _servicioUsuario.ObtenerRolesPorUsuario(usuarioId, HttpContext.RequestAborted);
            
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

        /// <summary>
        /// Obtener todos los roles
        /// </summary>
        [HttpGet("obtenerRoles")]
        public async Task<ActionResult<Usuario>> ObtenerRoles()
        {
            var resultado = await _servicioUsuario.ObtenerTodosLosRoles(HttpContext.RequestAborted);

            if (resultado.TieneErrores) return NotFound(new ProblemDetails
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

        /// <summary>
        /// Refrescar el token de acceso
        /// </summary>
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Token inválido",
                    Detail = "El token de refresco proporcionado no es válido.",
                    Status = 400
                });
            }

            var resultado = await _servicioUsuario.RefreshToken(refreshToken, HttpContext.RequestAborted);

            if (resultado.TieneErrores) return NotFound(new ProblemDetails
            {
                Title = "Error refresh token",
                Detail = "Ah ocurrido un error al intentar actualizar el token",
                Status = 404,
                Instance = HttpContext.Request.Path,
                Extensions = {
                        ["errors"] = resultado.Errores
                    }
            });

            (string nuevoAccessToken, string nuevoRefreshToken, string usuarioId) = resultado.Valor;
            LimpiarCookies();
            AgregarTokensACookies(nuevoAccessToken, nuevoRefreshToken);

            return Ok(new
            {
                accessToken = nuevoAccessToken,
                refreshToken = nuevoRefreshToken
            });
        }

        private void AgregarTokensACookies(string accessToken, string refreshToken)
        {
            var expiracionToken = _config.GetValue<int>("Jwt:ExpiracionAccessToken"); 
            var expiracionRefreshToken = _config.GetValue<int>("Jwt:ExpiracionRefreshToken");
            //var accessTokenCookieName = _config.GetValue<string>("Jwt:AccessTokenCookieName"); 
            //var refreshTokenCookieName = _config.GetValue<string>("Jwt:refreshTokenCookieName");
            
            Response.Cookies.Append(refreshTokenCookieName, refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(expiracionRefreshToken)
            });
            // Crear una cookie segura con HttpOnly y Secure activados, para tener mas seguridad.
            Response.Cookies.Append(accessTokenCookieName, accessToken, new CookieOptions
            {
                HttpOnly = true,  // No accesible desde el cliente
                Secure = true,    // Solo se enviará a través de HTTPS
                SameSite = SameSiteMode.None, // Evitar que se envíe en solicitudes de terceros
                Expires = DateTime.Now.AddMinutes(expiracionToken), 
                Path = "/"
            });

  
        }

        private void LimpiarCookies()
        {
            Response.Cookies.Append(accessTokenCookieName, "", new CookieOptions
            {
                HttpOnly = true,   // No accesible desde JavaScript
                Secure = true,     // Solo se enviará a través de HTTPS
                SameSite = SameSiteMode.None, // Asegura que no se envíe en solicitudes de terceros
                Expires = DateTime.Now.AddDays(-1), // Expira en 1 día para eliminarla
                Path = "/"         // El dominio de la cookie debe coincidir con el path original
            });

            Response.Cookies.Append(refreshTokenCookieName, "", new CookieOptions
            {
                HttpOnly = true,   // No accesible desde JavaScript
                Secure = true,     // Solo se enviará a través de HTTPS
                SameSite = SameSiteMode.None, // Asegura que no se envíe en solicitudes de terceros
                Expires = DateTime.Now.AddDays(-1), // Expira en 1 día para eliminarla
                Path = "/"         // El dominio de la cookie debe coincidir con el path original
            });
        }

    }
}
