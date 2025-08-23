using AutoMapper;
using Dominio;
using Dominio.Usuarios;
using Servicio.DTOS.SolicitudesDTO; // EnviarSolicitudDTO, SolicitudDTO, PaginacionSolicitudDTO
using Servicio.DTOS.GruposDTO; // GrupoDTO, UnirseAGrupoDTO
using Servicio.S_Grupos; // IServicioGrupos
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Servicio.Usuarios;
using System.Configuration;
using System.Net;

namespace FinHealthAPI.Controllers
{
    /// <summary>
    /// Controlador para la gestión de solicitudes.
    /// </summary>
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
        /// <summary>
        /// Obtiene las solicitudes de un administrador en particular.
        /// </summary>
        /// <param name="solicitudes">Objeto que contiene el id del administrador y el estado de las solicitudes.</param>
        /// <returns>Lista de solicitudes del administrador.</returns>
        [HttpGet("porAdmin")]
        public async Task<ActionResult<SolicitudDTO>> ObtenerSolicitudesPorAdministrador([FromBody] PaginacionSolicitudDTO solicitudes)
        {
            var resultado = await _servicioGrupo.ObtenerSolicitudesPorAdministrador(solicitudes.IdAdministrador, solicitudes.Estado, HttpContext.RequestAborted);

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
        /// <summary>
        /// Envía una solicitud para unirse a un grupo.
        /// </summary>
        /// <param name="enviarSolicitudDTO">Objeto que contiene la información de la solicitud.</param>
        /// <returns>El ID de la solicitud creada.</returns>
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

            var resultado = await _servicioGrupo.EnviarSolicitudIngresoAGrupo(enviarSolicitudDTO, HttpContext.RequestAborted);

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
        /// <summary>
        /// Acepta una solicitud de unión a un grupo.
        /// </summary>
        /// <param name="idSolicitud">ID de la solicitud a aceptar.</param>
        /// <returns>Indica si la aceptación fue exitosa.</returns>
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

            var resultado = await _servicioGrupo.AceptarSolicitudIngresoAGrupo(idSolicitud, HttpContext.RequestAborted);

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
        /// <summary>
        /// Se une a un grupo utilizando un código.
        /// </summary>
        /// <param name="solicitud">Objeto que contiene la información para unirse al grupo.</param>
        /// <returns>Indica si la unión al grupo fue exitosa.</returns>
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

            var resultado = await _servicioGrupo.IngresoAGrupoConCodigo(solicitud, HttpContext.RequestAborted);

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
