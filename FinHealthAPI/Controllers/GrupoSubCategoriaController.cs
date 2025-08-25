using Microsoft.AspNetCore.Mvc;
using Servicio.S_Grupos;
using Servicio.DTOS.SubCategoriasDTO;
using System.Threading;
using System.Threading.Tasks;

namespace FinHealthAPI.Controllers
{
    [ApiController]
    [Route("api/grupos/{grupoId}/subcategorias")]
    public class GrupoSubCategoriaController : ControllerBase
    {
        private readonly IServicioGrupoSubCategoria _servicioGrupoSubCategoria;

        public GrupoSubCategoriaController(IServicioGrupoSubCategoria servicioGrupoSubCategoria)
        {
            _servicioGrupoSubCategoria = servicioGrupoSubCategoria;
        }

        /// <summary>
        /// Obtiene todas las subcategorías personalizadas de un grupo.
        /// </summary>
        /// <param name="grupoId">ID del grupo.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Lista de subcategorías personalizadas.</returns>
        /// <response code="200">Lista de subcategorías del grupo.</response>
        /// <response code="404">No se encontraron subcategorías para el grupo.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GrupoSubCategoriaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<GrupoSubCategoriaDTO>>> GetSubCategorias(int grupoId, CancellationToken cancellationToken)
        {
            var resultado = await _servicioGrupoSubCategoria.ObtenerPorGrupoIdAsync(grupoId, cancellationToken);
            if (resultado.TieneErrores)
                return NotFound(new ProblemDetails
                {
                    Title = "Error al obtener subcategorías del grupo",
                    Detail = resultado.ObtenerErroresComoString(),
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });
            return Ok(resultado.Valor);
        }

        /// <summary>
        /// Crea una nueva subcategoría personalizada para el grupo.
        /// </summary>
        /// <param name="grupoId">ID del grupo.</param>
        /// <param name="dto">Datos de la subcategoría a crear.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Subcategoría creada.</returns>
        /// <response code="201">Subcategoría creada correctamente.</response>
        /// <response code="400">Datos inválidos o error de negocio.</response>
        [HttpPost]
        [ProducesResponseType(typeof(GrupoSubCategoriaDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GrupoSubCategoriaDTO>> CrearSubCategoria(int grupoId, [FromBody] CrearGrupoSubCategoriaDTO dto, CancellationToken cancellationToken)
        {
            if (dto == null || dto.SubCategoriaId <= 0)
                return BadRequest(new ProblemDetails
                {
                    Title = "Datos de subcategoría inválidos",
                    Detail = "El cuerpo de la solicitud es inválido, contiene errores de validación.",
                    Status = StatusCodes.Status400BadRequest,
                    Instance = HttpContext.Request.Path
                });
            var resultado = await _servicioGrupoSubCategoria.CrearAsync(grupoId, dto, cancellationToken);
            if (resultado.TieneErrores)
                return BadRequest(new ProblemDetails
                {
                    Title = "Error al crear subcategoría en grupo",
                    Detail = resultado.ObtenerErroresComoString(),
                    Status = StatusCodes.Status400BadRequest,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });
            return CreatedAtAction(nameof(GetSubCategorias), new { grupoId }, resultado.Valor);
        }

        /// <summary>
        /// Actualiza una subcategoría personalizada de un grupo.
        /// </summary>
        /// <param name="grupoId">ID del grupo.</param>
        /// <param name="id">ID de la subcategoría personalizada.</param>
        /// <param name="dto">Datos a actualizar.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Subcategoría actualizada.</returns>
        /// <response code="200">Subcategoría actualizada correctamente.</response>
        /// <response code="400">Error de validación o negocio.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(GrupoSubCategoriaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GrupoSubCategoriaDTO>> ActualizarSubCategoria(int grupoId, int id, [FromBody] ActualizarGrupoSubCategoriaDTO dto, CancellationToken cancellationToken)
        {
            var resultado = await _servicioGrupoSubCategoria.ActualizarAsync(grupoId, id, dto, cancellationToken);
            if (resultado.TieneErrores)
                return BadRequest(new ProblemDetails
                {
                    Title = "Error al actualizar subcategoría en grupo",
                    Detail = resultado.ObtenerErroresComoString(),
                    Status = StatusCodes.Status400BadRequest,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });
            return Ok(resultado.Valor);
        }

        /// <summary>
        /// Elimina una subcategoría personalizada de un grupo.
        /// </summary>
        /// <param name="grupoId">ID del grupo.</param>
        /// <param name="id">ID de la subcategoría personalizada.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Sin contenido si la eliminación fue exitosa.</returns>
        /// <response code="204">Eliminación exitosa.</response>
        /// <response code="400">Error de negocio.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> EliminarSubCategoria(int grupoId, int id, CancellationToken cancellationToken)
        {
            var resultado = await _servicioGrupoSubCategoria.EliminarAsync(grupoId, id, cancellationToken);
            if (resultado.TieneErrores)
                return BadRequest(new ProblemDetails
                {
                    Title = "Error al eliminar subcategoría en grupo",
                    Detail = resultado.ObtenerErroresComoString(),
                    Status = StatusCodes.Status400BadRequest,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });
            return NoContent();
        }
    }
}
