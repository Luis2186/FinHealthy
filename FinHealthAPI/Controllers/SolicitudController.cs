using AutoMapper;
using Dominio;
using Dominio.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicio.DTOS.GrupoDTO;
using Servicio.DTOS.SolicitudesDTO;
using Servicio.DTOS.UsuariosDTO;
using Servicio.Notificaciones.NotificacionesDTO;
using Servicio.Pdf;
using Servicio.S_Grupos;

namespace FinHealthAPI.Controllers
{
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
        [HttpGet("porAdmin")]
        public async Task<ActionResult<SolicitudDTO>> ObtenerSolicitudesPorAdministrador([FromBody] PaginacionSolicitudDTO solicitudes)
        {
            var resultado = await _servicioGrupo.ObtenerSolicitudesPorAdministrador(solicitudes.IdAdministrador,solicitudes.Estado);

            if (resultado.TieneErrores) return NotFound(new ProblemDetails
            {
                Title = "Error solicitudes por administrador",
                Detail = "Hubo un error al intentar obtener las solicitudes por administrador",
                Status = 404,
                Instance = HttpContext.Request.Path,
                Extensions = {
                        ["errors"] = resultado.Errores
                    }
            });

            return Ok(resultado.Valor);  // Devuelve los datos con estado HTTP 200 OK
        }

        // Obtener un usuario por su ID
        //[HttpGet("obtener/{solicitudId}")]
        //public async Task<ActionResult<GrupoDTO>> ObtenerSolicitudPorId(int solicitudId)
        //{
        //    var resultado = await _servicioGrupo.ObtenerGrupoPorId(grupoId);

        //    if (resultado.TieneErrores)
        //    {
        //        return NotFound(new ProblemDetails
        //        {
        //            Title = "Error al obtener usuario por id",
        //            Detail = "Hubo un error al obtener el usuario por id",
        //            Status = 404,
        //            Instance = HttpContext.Request.Path,
        //            Extensions = {
        //                ["errors"] = resultado.Errores
        //            }
        //        });  // Devuelve 404 si no se encuentra la grupo
        //    }

        //    return Ok(resultado.Valor);  // Devuelve el usuario con estado 200 OK
        //}

        // Crear un nuevo usuario
        [HttpPost("enviar")]
        public async Task<ActionResult<GrupoDTO>> EnviarSolicitud([FromBody] EnviarSolicitudDTO enviarSolicitudDTO)
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

            var resultado = await _servicioGrupo.EnviarSolicitudIngresoAGrupo(enviarSolicitudDTO);

            // En caso de que el usuario ya exista o haya un error, devolver BadRequest
            if (resultado.TieneErrores)
            {
                return Conflict(new ProblemDetails
                {
                    Title = "Error al enviar la solicitud",
                    Detail = "Ah ocurrido un error cuando se intento enviar la solicitud",
                    Status = 404,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = resultado.Errores
                    }
                });
            }

            return Ok(new { solicitudId = resultado.Valor.Id });
        }

        // Crear un nuevo usuario
        [HttpPost("aceptar/{idSolicitud}")]
        public async Task<ActionResult<bool>> AceptarSolicitudDeUnionGrupo(int idSolicitud)
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

            var resultado = await _servicioGrupo.AceptarSolicitudIngresoAGrupo(idSolicitud);

            // En caso de que el usuario ya exista o haya un error, devolver BadRequest
            if (resultado.TieneErrores)
            {
                return Conflict(new ProblemDetails
                {
                    Title = "Error al aceptar la solicitud",
                    Detail = "Ah ocurrido un error cuando se intento aceptar la solicitud",
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
        [HttpPost("porCodigo")]
        public async Task<ActionResult<bool>> UnirseConCodigoAGrupo([FromBody] UnirseAGrupoDTO solicitud)
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

            var resultado = await _servicioGrupo.IngresoAGrupoConCodigo(solicitud);

            // En caso de que el usuario ya exista o haya un error, devolver BadRequest
            if (resultado.TieneErrores)
            {
                return Conflict(new ProblemDetails
                {
                    Title = "Error al unirse al grupo por medio del codigo",
                    Detail = "Ah ocurrido un error al intentar unirse al grupo por medio del codigo",
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
