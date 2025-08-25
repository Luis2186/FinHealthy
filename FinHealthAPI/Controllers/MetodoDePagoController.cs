using AutoMapper;
using Dominio;
using Dominio.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Servicio.DTOS.MetodosDePagoDTO;
using Servicio.S_MetodosDePago;
using Servicio.Usuarios;

namespace FinHealthAPI.Controllers
{
    /// <summary>
    /// Controlador para la gestión de métodos de pago.
    /// </summary>
    [ApiController]
    [Route("api/metodos-de-pago")]
    public class MetodoDePagoController : ControllerBase
    {
        private readonly IServicioMetodoDePago _servicio;

        public MetodoDePagoController(IServicioMetodoDePago servicio)
        {
            _servicio = servicio;
        }

        /// <summary>
        /// Obtiene todos los métodos de pago.
        /// </summary>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Una lista de métodos de pago.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MetodoDePagoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<MetodoDePagoDTO>>> GetAll(CancellationToken ct)
        {
            var result = await _servicio.ObtenerTodosAsync(ct);
            if (result.TieneErrores)
                return BadRequest(new ProblemDetails
                {
                    Title = "Error al obtener métodos de pago",
                    Detail = result.ObtenerErroresComoString(),
                    Status = StatusCodes.Status400BadRequest
                });
            return Ok(result.Valor);
        }

        /// <summary>
        /// Obtiene un método de pago por su ID.
        /// </summary>
        /// <param name="id">ID del método de pago.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>El método de pago solicitado.</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(MetodoDePagoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MetodoDePagoDTO>> GetById(int id, CancellationToken ct)
        {
            var result = await _servicio.ObtenerPorIdAsync(id, ct);
            if (result.TieneErrores)
                return NotFound(new ProblemDetails
                {
                    Title = "Método de pago no encontrado",
                    Detail = result.ObtenerErroresComoString(),
                    Status = StatusCodes.Status404NotFound
                });
            return Ok(result.Valor);
        }

        /// <summary>
        /// Crea un nuevo método de pago.
        /// </summary>
        /// <param name="dto">Datos del método de pago a crear.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>El método de pago creado.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(MetodoDePagoDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MetodoDePagoDTO>> Create([FromBody] CrearMetodoDePagoDTO dto, CancellationToken ct)
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
                    Title = "Error al crear método de pago",
                    Detail = result.ObtenerErroresComoString(),
                    Status = StatusCodes.Status400BadRequest
                });
            return CreatedAtAction(nameof(GetById), new { id = result.Valor.Id }, result.Valor);
        }

        /// <summary>
        /// Actualiza un método de pago existente.
        /// </summary>
        /// <param name="id">ID del método de pago a actualizar.</param>
        /// <param name="dto">Datos actualizados del método de pago.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>El método de pago actualizado.</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(MetodoDePagoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MetodoDePagoDTO>> Update(int id, [FromBody] ActualizarMetodoDePagoDTO dto, CancellationToken ct)
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
                    Title = "Error al actualizar método de pago",
                    Detail = result.ObtenerErroresComoString(),
                    Status = StatusCodes.Status400BadRequest
                });
            return Ok(result.Valor);
        }

        /// <summary>
        /// Elimina un método de pago por su ID.
        /// </summary>
        /// <param name="id">ID del método de pago a eliminar.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>No content si la eliminación fue exitosa.</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _servicio.EliminarAsync(id, ct);
            if (result.TieneErrores)
                return BadRequest(new ProblemDetails
                {
                    Title = "Error al eliminar método de pago",
                    Detail = result.ObtenerErroresComoString(),
                    Status = StatusCodes.Status400BadRequest
                });
            return NoContent();
        }
    }
}
