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
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await _servicio.ObtenerTodosAsync(ct);
            if (result.TieneErrores)
                return Problem(detail: result.ObtenerErroresComoString(), statusCode: 400);
            return Ok(result.Valor);
        }

        /// <summary>
        /// Obtiene un método de pago por su ID.
        /// </summary>
        /// <param name="id">ID del método de pago.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>El método de pago solicitado.</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await _servicio.ObtenerPorIdAsync(id, ct);
            if (result.TieneErrores)
                return NotFound(result.ObtenerErroresComoString());
            return Ok(result.Valor);
        }

        /// <summary>
        /// Crea un nuevo método de pago.
        /// </summary>
        /// <param name="dto">Datos del método de pago a crear.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>El método de pago creado.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CrearMetodoDePagoDTO dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            var result = await _servicio.CrearAsync(dto, ct);
            if (result.TieneErrores)
                return Problem(detail: result.ObtenerErroresComoString(), statusCode: 400);
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
        public async Task<IActionResult> Update(int id, [FromBody] ActualizarMetodoDePagoDTO dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            var result = await _servicio.ActualizarAsync(id, dto, ct);
            if (result.TieneErrores)
                return Problem(detail: result.ObtenerErroresComoString(), statusCode: 400);
            return Ok(result.Valor);
        }

        /// <summary>
        /// Elimina un método de pago por su ID.
        /// </summary>
        /// <param name="id">ID del método de pago a eliminar.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>No content si la eliminación fue exitosa.</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _servicio.EliminarAsync(id, ct);
            if (result.TieneErrores)
                return Problem(detail: result.ObtenerErroresComoString(), statusCode: 400);
            return NoContent();
        }
    }
}
