using AutoMapper;
using Dominio;
using Dominio.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Servicio.DTOS.TipoDeDocumentoDTO;
using Servicio.S_TipoDeDocumento;
using Servicio.Usuarios;

namespace FinHealthAPI.Controllers
{
    /// <summary>
    /// Controlador para la gestión de tipos de documento.
    /// </summary>
    [ApiController]
    [Route("api/tipo-de-documento")]
    public class TipoDeDocumentoController : ControllerBase
    {
        private readonly IServicioTipoDeDocumento _servicio;

        /// <summary>
        /// Constructor para el controlador de tipos de documento.
        /// </summary>
        /// <param name="servicio">Servicio para la gestión de tipos de documento.</param>
        public TipoDeDocumentoController(IServicioTipoDeDocumento servicio)
        {
            _servicio = servicio;
        }

        /// <summary>
        /// Obtiene todos los tipos de documento.
        /// </summary>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Lista de tipos de documento.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TipoDeDocumentoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TipoDeDocumentoDTO>>> GetAll(CancellationToken ct)
        {
            var result = await _servicio.ObtenerTodosAsync(ct);
            if (result.TieneErrores)
                return BadRequest(new ProblemDetails
                {
                    Title = "Error al obtener tipos de documento",
                    Detail = result.ObtenerErroresComoString(),
                    Status = StatusCodes.Status400BadRequest
                });
            return Ok(result.Valor);
        }

        /// <summary>
        /// Obtiene un tipo de documento por su ID.
        /// </summary>
        /// <param name="id">ID del tipo de documento.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Tipo de documento solicitado.</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(TipoDeDocumentoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TipoDeDocumentoDTO>> GetById(int id, CancellationToken ct)
        {
            var result = await _servicio.ObtenerPorIdAsync(id, ct);
            if (result.TieneErrores)
                return NotFound(new ProblemDetails
                {
                    Title = "Tipo de documento no encontrado",
                    Detail = result.ObtenerErroresComoString(),
                    Status = StatusCodes.Status404NotFound
                });
            return Ok(result.Valor);
        }

        /// <summary>
        /// Crea un nuevo tipo de documento.
        /// </summary>
        /// <param name="dto">Datos del tipo de documento a crear.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Resultado de la creación del tipo de documento.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TipoDeDocumentoDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TipoDeDocumentoDTO>> Create([FromBody] CrearTipoDeDocumentoDTO dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ProblemDetails
                {
                    Title = "Error de validación",
                    Detail = "El cuerpo de la solicitud es inválido, contiene errores de validación.",
                    Status = StatusCodes.Status400BadRequest
                });
            var result = await _servicio.CrearAsync(dto, ct);
            if (result.TieneErrores)
                return BadRequest(new ProblemDetails
                {
                    Title = "Error al crear tipo de documento",
                    Detail = result.ObtenerErroresComoString(),
                    Status = StatusCodes.Status400BadRequest
                });
            return CreatedAtAction(nameof(GetById), new { id = result.Valor.Id }, result.Valor);
        }

        /// <summary>
        /// Actualiza un tipo de documento existente.
        /// </summary>
        /// <param name="id">ID del tipo de documento a actualizar.</param>
        /// <param name="dto">Datos actualizados del tipo de documento.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Resultado de la actualización del tipo de documento.</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(TipoDeDocumentoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TipoDeDocumentoDTO>> Update(int id, [FromBody] ActualizarTipoDeDocumentoDTO dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ProblemDetails
                {
                    Title = "Error de validación",
                    Detail = "El cuerpo de la solicitud es inválido, contiene errores de validación.",
                    Status = StatusCodes.Status400BadRequest
                });
            var result = await _servicio.ActualizarAsync(id, dto, ct);
            if (result.TieneErrores)
                return BadRequest(new ProblemDetails
                {
                    Title = "Error al actualizar tipo de documento",
                    Detail = result.ObtenerErroresComoString(),
                    Status = StatusCodes.Status400BadRequest
                });
            return Ok(result.Valor);
        }

        /// <summary>
        /// Elimina un tipo de documento por su ID.
        /// </summary>
        /// <param name="id">ID del tipo de documento a eliminar.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Resultado de la eliminación del tipo de documento.</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _servicio.EliminarAsync(id, ct);
            if (result.TieneErrores)
                return BadRequest(new ProblemDetails
                {
                    Title = "Error al eliminar tipo de documento",
                    Detail = result.ObtenerErroresComoString(),
                    Status = StatusCodes.Status400BadRequest
                });
            return NoContent();
        }
    }
}
