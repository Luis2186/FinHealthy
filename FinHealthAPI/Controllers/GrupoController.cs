using AutoMapper;
using Dominio;
using Dominio.Usuarios;
using Dominio.Grupos; // Grupo
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Servicio.DTOS.GruposDTO; // GrupoDTO
using Servicio.S_Grupos; // IServicioGrupos
using Servicio.Usuarios;
using System.Configuration;
using System.Net;

namespace FinHealthAPI.Controllers
{
    /// <summary>
    /// Controlador para la gestión de grupos familiares.
    /// </summary>
    [Authorize(Roles = "Sys_Adm , Administrador, Usuario")]
    [ApiController]
    [Route("/grupo")]
    public class GrupoController : Controller
    {
        private readonly IServicioGrupos _servicioGrupos;
        private readonly IMapper _mapper;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="GrupoController"/>.
        /// </summary>
        /// <param name="servicioGrupo">Servicio de grupos.</param>
        /// <param name="mapper">Instancia de AutoMapper.</param>
        public GrupoController(IServicioGrupos servicioGrupo, IMapper mapper)
        {
            _servicioGrupos = servicioGrupo;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene todos los grupos familiares.
        /// </summary>
        /// <returns>Lista de grupos.</returns>
        /// <response code="200">Retorna la lista de grupos.</response>
        /// <response code="404">Si ocurre un error al obtener los grupos.</response>
        [HttpGet("todas")]
        [ProducesResponseType(typeof(IEnumerable<GrupoDTO>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<ActionResult<Grupo>> ObtenerGrupos()
        {
            var resultado = await _servicioGrupos.ObtenerTodosLosGrupos(HttpContext.RequestAborted);

            if (resultado.TieneErrores) return NotFound(new ProblemDetails
            {
                Title = "Error al obtener todos los grupos",
                Detail = "Ah ocurrido un error al intentar obtener todos los grupos",
                Status = 404,
                Instance = HttpContext.Request.Path,
                Extensions = {
                        ["errors"] = resultado.Errores
                    }
            });

            return Ok(resultado.Valor);  // Devuelve los datos con estado HTTP 200 OK
        }

        /// <summary>
        /// Obtiene todos los grupos de un usuario específico.
        /// </summary>
        /// <param name="idUsuario">ID del usuario.</param>
        /// <returns>Lista de grupos del usuario.</returns>
        /// <response code="200">Retorna la lista de grupos del usuario.</response>
        /// <response code="404">Si ocurre un error al obtener los grupos.</response>
        [HttpGet("todas/{idUsuario}")]
        [ProducesResponseType(typeof(IEnumerable<GrupoDTO>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<ActionResult<Grupo>> ObtenerGrupos(string idUsuario)
        {
            var resultado = await _servicioGrupos.ObtenerTodosLosGruposPorUsuario(idUsuario, HttpContext.RequestAborted);

            if (resultado.TieneErrores) return NotFound(new ProblemDetails
            {
                Title = "Error al obtener todos los grupos",
                Detail = "Ah ocurrido un error al intentar obtener todos los grupos del usuario" + idUsuario,
                Status = 404,
                Instance = HttpContext.Request.Path,
                Extensions = {
                        ["errors"] = resultado.Errores
                    }
            });

            return Ok(resultado.Valor);  // Devuelve los datos con estado HTTP 200 OK
        }

        /// <summary>
        /// Obtiene un grupo por su ID.
        /// </summary>
        /// <param name="grupoId">ID del grupo.</param>
        /// <returns>Grupo encontrado.</returns>
        /// <response code="200">Retorna el grupo.</response>
        /// <response code="404">Si no se encuentra el grupo.</response>
        [HttpGet("obtener/{grupoId}")]
        [ProducesResponseType(typeof(GrupoDTO), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<ActionResult<GrupoDTO>> ObtenerGrupoPorId(int grupoId)
        {
            var resultado = await _servicioGrupos.ObtenerGrupoPorId(grupoId, HttpContext.RequestAborted);

            if (resultado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error al obtener el grupo por id",
                    Detail = "Ah ocurrido un error al intentar obtener el grupo por id",
                    Status = 404,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = resultado.Errores
                    }
                });  // Devuelve 404 si no se encuentra la grupo
            }

            return Ok(resultado.Valor);  // Devuelve el usuario con estado 200 OK
        }

        /// <summary>
        /// Crea un nuevo grupo familiar.
        /// </summary>
        /// <param name="grupoCreacionDTO">DTO con los datos del grupo a crear.</param>
        /// <returns>ID del grupo creado.</returns>
        /// <response code="200">Retorna el ID del grupo creado.</response>
        /// <response code="400">Si la solicitud es inválida.</response>
        /// <response code="409">Si ocurre un error al crear el grupo.</response>
        [HttpPost("crear")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 409)]
        public async Task<ActionResult<GrupoDTO>> CrearGrupo([FromBody] CrearGrupoDTO grupoCreacionDTO)
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

            var resultado = await _servicioGrupos.CrearGrupo(grupoCreacionDTO, HttpContext.RequestAborted);

            // En caso de que el usuario ya exista o haya un error, devolver BadRequest
            if (resultado.TieneErrores)
            {
                return Conflict(new ProblemDetails
                {
                    Title = "Error al crear el grupo",
                    Detail = "Ah ocurrido un error al intentar crear el grupo",
                    Status = 409,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = resultado.Errores
                    }
                });
            }

            return Ok(new { grupoId = resultado.Valor});
        }

        /// <summary>
        /// Actualiza los datos de un grupo familiar.
        /// </summary>
        /// <param name="grupoId">ID del grupo a actualizar.</param>
        /// <param name="grupoActDTO">DTO con los datos actualizados.</param>
        /// <returns>Grupo actualizado.</returns>
        /// <response code="200">Retorna el grupo actualizado.</response>
        /// <response code="400">Si la solicitud es inválida.</response>
        /// <response code="404">Si ocurre un error al actualizar el grupo.</response>
        [HttpPut("actualizar/{grupoId}")]
        [ProducesResponseType(typeof(GrupoDTO), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<ActionResult<GrupoDTO>> ActualizarGrupo(int grupoId, [FromBody] ActualizarGrupoDTO grupoActDTO)
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

            var resultado = await _servicioGrupos.ActualizarGrupo(grupoId, grupoActDTO, HttpContext.RequestAborted);

            if (resultado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error al actualizar el grupo",
                    Detail = "Ah ocurrido un error al intentar actualizar el grupo",
                    Status = 404,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = resultado.Errores
                    }
                });
            }

            return Ok(resultado.Valor);  // Devuelve el usuario actualizado con estado 200 OK
        }

        /// <summary>
        /// Elimina un grupo familiar por su ID.
        /// </summary>
        /// <param name="grupoId">ID del grupo a eliminar.</param>
        /// <returns>Sin contenido si la eliminación fue exitosa.</returns>
        /// <response code="204">El grupo fue eliminado correctamente.</response>
        /// <response code="404">Si ocurre un error al eliminar el grupo.</response>
        [HttpDelete("eliminar/{grupoId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<ActionResult> EliminarGrupo(int grupoId)
        {
            var resultado = await _servicioGrupos.EliminarGrupo(grupoId, HttpContext.RequestAborted);

            if (resultado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error al eliminar grupo",
                    Detail = "Ah ocurrido un error al intentar eliminar el grupo",
                    Status = 404,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = resultado.Errores
                    }
                });
            }

            return NoContent();  // Devuelve 204 No Content si la eliminación fue exitosa
        }


    }
}
