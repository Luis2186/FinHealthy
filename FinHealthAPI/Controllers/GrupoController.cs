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
        [ProducesResponseType(typeof(IEnumerable<GrupoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<GrupoDTO>>> ObtenerGrupos(CancellationToken cancellationToken)
        {
            var resultado = await _servicioGrupos.ObtenerTodosLosGrupos(cancellationToken);

            if (resultado.TieneErrores)
                return NotFound(new ProblemDetails
                {
                    Title = "Error al obtener todos los grupos",
                    Detail = "Ah ocurrido un error al intentar obtener todos los grupos",
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });

            return Ok(resultado.Valor);
        }

        /// <summary>
        /// Obtiene todos los grupos de un usuario específico.
        /// </summary>
        /// <param name="idUsuario">ID del usuario.</param>
        /// <returns>Lista de grupos del usuario.</returns>
        /// <response code="200">Retorna la lista de grupos del usuario.</response>
        /// <response code="404">Si ocurre un error al obtener los grupos.</response>
        [HttpGet("todas/{idUsuario}")]
        [ProducesResponseType(typeof(IEnumerable<GrupoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<GrupoDTO>>> ObtenerGruposPorUsuario(string idUsuario, CancellationToken cancellationToken)
        {
            var resultado = await _servicioGrupos.ObtenerTodosLosGruposPorUsuario(idUsuario, cancellationToken);

            if (resultado.TieneErrores)
                return NotFound(new ProblemDetails
                {
                    Title = "Error al obtener todos los grupos",
                    Detail = $"Ah ocurrido un error al intentar obtener todos los grupos del usuario {idUsuario}",
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });

            return Ok(resultado.Valor);
        }

        /// <summary>
        /// Obtiene un grupo por su ID.
        /// </summary>
        /// <param name="grupoId">ID del grupo.</param>
        /// <returns>Grupo encontrado.</returns>
        /// <response code="200">Retorna el grupo.</response>
        /// <response code="404">Si no se encuentra el grupo.</response>
        [HttpGet("obtener/{grupoId}")]
        [ProducesResponseType(typeof(GrupoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GrupoDTO>> ObtenerGrupoPorId(int grupoId, CancellationToken cancellationToken)
        {
            var resultado = await _servicioGrupos.ObtenerGrupoPorId(grupoId, cancellationToken);

            if (resultado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error al obtener el grupo por id",
                    Detail = "Ah ocurrido un error al intentar obtener el grupo por id",
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });
            }

            return Ok(resultado.Valor);
        }

        /// <summary>
        /// Crea un nuevo grupo familiar.
        /// </summary>
        /// <param name="grupoCreacionDTO">DTO con los datos del grupo a crear.</param>
        /// <returns>ID del grupo creado.</returns>
        /// <response code="200">Retorna el ID del grupo creado.</response>
        /// <response code="400">Si la solicitud es inválida.</response>
        public async Task<ActionResult<GrupoDTO>> CrearGrupo([FromBody] CrearGrupoDTO grupoCreacionDTO, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Error de validacion",
                    Detail = "El cuerpo de la solicitud es invalido, contiene errores de validacion.",
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = ModelState.Keys
                        .SelectMany(key => ModelState[key].Errors.Select(error => new
                        {
                            Code = key,
                            Description = error.ErrorMessage
                        })) }
                });
            }

            var resultado = await _servicioGrupos.CrearGrupo(grupoCreacionDTO, cancellationToken);

            if (resultado.TieneErrores)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Error al crear el grupo",
                    Detail = "Ah ocurrido un error al intentar crear el grupo",
                    Status = StatusCodes.Status400BadRequest,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });
            }

            return CreatedAtAction(nameof(CrearGrupo), new { id = resultado.Valor }, resultado.Valor);
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
        [ProducesResponseType(typeof(GrupoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GrupoDTO>> ActualizarGrupo(int grupoId, [FromBody] ActualizarGrupoDTO grupoActDTO, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Error de validacion",
                    Detail = "El cuerpo de la solicitud es invalido, contiene errores de validacion.",
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = ModelState.Keys
                        .SelectMany(key => ModelState[key].Errors.Select(error => new
                        {
                            Code = key,
                            Description = error.ErrorMessage
                        })) }
                });
            }

            var resultado = await _servicioGrupos.ActualizarGrupo(grupoId, grupoActDTO, cancellationToken);

            if (resultado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error al actualizar el grupo",
                    Detail = "Ah ocurrido un error al intentar actualizar el grupo",
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });
            }

            return Ok(resultado.Valor);
        }

        /// <summary>
        /// Elimina un grupo familiar por su ID.
        /// </summary>
        /// <param name="grupoId">ID del grupo a eliminar.</param>
        /// <returns>Sin contenido si la eliminación fue exitosa.</returns>
        /// <response code="204">El grupo fue eliminado correctamente.</response>
        /// <response code="404">Si ocurre un error al eliminar el grupo.</response>
        [HttpDelete("eliminar/{grupoId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> EliminarGrupo(int grupoId, CancellationToken cancellationToken)
        {
            var resultado = await _servicioGrupos.EliminarGrupo(grupoId, cancellationToken);

            if (resultado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error al eliminar grupo",
                    Detail = "Ah ocurrido un error al intentar eliminar el grupo",
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });
            }

            return NoContent();
        }


    }
}
