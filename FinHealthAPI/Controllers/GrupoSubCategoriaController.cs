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
        /// Obtiene todas las subcategor�as personalizadas de un grupo.
        /// </summary>
        /// <param name="grupoId">ID del grupo.</param>
        /// <param name="cancellationToken">Token de cancelaci�n.</param>
        /// <returns>Lista de subcategor�as personalizadas.</returns>
        /// <response code="200">Lista de subcategor�as del grupo.</response>
        /// <response code="404">No se encontraron subcategor�as para el grupo.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GrupoSubCategoriaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<GrupoSubCategoriaDTO>>> GetSubCategorias(int grupoId, CancellationToken cancellationToken)
        {
            var resultado = await _servicioGrupoSubCategoria.ObtenerPorGrupoIdAsync(grupoId, cancellationToken);
            if (resultado.TieneErrores)
                return NotFound(new ProblemDetails
                {
                    Title = "Error al obtener subcategor�as del grupo",
                    Detail = resultado.ObtenerErroresComoString(),
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });
            return Ok(resultado.Valor);
        }

        /// <summary>
        /// Crea una nueva subcategor�a personalizada para el grupo.
        /// </summary>
        /// <param name="grupoId">ID del grupo.</param>
        /// <param name="dto">Datos de la subcategor�a a crear.</param>
        /// <param name="cancellationToken">Token de cancelaci�n.</param>
        /// <returns>Subcategor�a creada.</returns>
        /// <response code="201">Subcategor�a creada correctamente.</response>
        /// <response code="400">Datos inv�lidos o error de negocio.</response>
        [HttpPost]
        [ProducesResponseType(typeof(GrupoSubCategoriaDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GrupoSubCategoriaDTO>> CrearSubCategoria(int grupoId, [FromBody] CrearGrupoSubCategoriaDTO dto, CancellationToken cancellationToken)
        {
            if (dto == null || dto.SubCategoriaId <= 0)
                return BadRequest(new ProblemDetails
                {
                    Title = "Datos de subcategor�a inv�lidos",
                    Detail = "El cuerpo de la solicitud es inv�lido, contiene errores de validaci�n.",
                    Status = StatusCodes.Status400BadRequest,
                    Instance = HttpContext.Request.Path
                });
            var resultado = await _servicioGrupoSubCategoria.CrearAsync(grupoId, dto, cancellationToken);
            if (resultado.TieneErrores)
                return BadRequest(new ProblemDetails
                {
                    Title = "Error al crear subcategor�a en grupo",
                    Detail = resultado.ObtenerErroresComoString(),
                    Status = StatusCodes.Status400BadRequest,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });
            return CreatedAtAction(nameof(GetSubCategorias), new { grupoId }, resultado.Valor);
        }

        /// <summary>
        /// Actualiza una subcategor�a personalizada de un grupo.
        /// </summary>
        /// <param name="grupoId">ID del grupo.</param>
        /// <param name="id">ID de la subcategor�a personalizada.</param>
        /// <param name="dto">Datos a actualizar.</param>
        /// <param name="cancellationToken">Token de cancelaci�n.</param>
        /// <returns>Subcategor�a actualizada.</returns>
        /// <response code="200">Subcategor�a actualizada correctamente.</response>
        /// <response code="400">Error de validaci�n o negocio.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(GrupoSubCategoriaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GrupoSubCategoriaDTO>> ActualizarSubCategoria(int grupoId, int id, [FromBody] ActualizarGrupoSubCategoriaDTO dto, CancellationToken cancellationToken)
        {
            var resultado = await _servicioGrupoSubCategoria.ActualizarAsync(grupoId, id, dto, cancellationToken);
            if (resultado.TieneErrores)
                return BadRequest(new ProblemDetails
                {
                    Title = "Error al actualizar subcategor�a en grupo",
                    Detail = resultado.ObtenerErroresComoString(),
                    Status = StatusCodes.Status400BadRequest,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });
            return Ok(resultado.Valor);
        }

        /// <summary>
        /// Elimina una subcategor�a personalizada de un grupo.
        /// </summary>
        /// <param name="grupoId">ID del grupo.</param>
        /// <param name="id">ID de la subcategor�a personalizada.</param>
        /// <param name="cancellationToken">Token de cancelaci�n.</param>
        /// <returns>Sin contenido si la eliminaci�n fue exitosa.</returns>
        /// <response code="204">Eliminaci�n exitosa.</response>
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
                    Title = "Error al eliminar subcategor�a en grupo",
                    Detail = resultado.ObtenerErroresComoString(),
                    Status = StatusCodes.Status400BadRequest,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });
            return NoContent();
        }
    }
}
