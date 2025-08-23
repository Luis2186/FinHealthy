using Microsoft.AspNetCore.Mvc;
using Servicio.S_MetodosDePago;
using Servicio.DTOS.MetodosDePagoDTO;
using System.Threading;
using System.Threading.Tasks;

namespace FinHealthAPI.Controllers
{
    [ApiController]
    [Route("api/metodos-de-pago")]
    public class MetodoDePagoController : ControllerBase
    {
        private readonly IServicioMetodoDePago _servicio;

        public MetodoDePagoController(IServicioMetodoDePago servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await _servicio.ObtenerTodosAsync(ct);
            if (result.TieneErrores)
                return Problem(detail: result.ObtenerErroresComoString(), statusCode: 400);
            return Ok(result.Valor);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await _servicio.ObtenerPorIdAsync(id, ct);
            if (result.TieneErrores)
                return NotFound(result.ObtenerErroresComoString());
            return Ok(result.Valor);
        }

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
