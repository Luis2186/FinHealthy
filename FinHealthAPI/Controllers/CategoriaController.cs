using AutoMapper;
using Dominio.Familias;
using Dominio.Gastos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Servicio.DTOS.FamiliasDTO;
using Servicio.S_Familias;
using Servicio.S_Gastos.S_Categoria;

namespace FinHealthAPI.Controllers
{
    [Authorize(Roles = "Sys_Adm , Administrador, Usuario")]
    [ApiController]
    [Route("/categoria")]
    public class CategoriaController : Controller
    {
        private readonly IServicioCategoria _servicioCategoria;
        private readonly IMapper _mapper;

        public CategoriaController(IServicioCategoria servicioCategoria, IMapper mapper)
        {
            _servicioCategoria = servicioCategoria;
            _mapper = mapper;
        }


        // Obtener todos los usuarios con paginación
        [HttpGet("todas")]
        public async Task<ActionResult<Categoria>> ObtenerTodas()
        {
            var resultado = await _servicioCategoria.ObtenerTodosAsync();

            if (resultado.TieneErrores) return NotFound(resultado.Errores);

            return Ok(resultado.Valor);  // Devuelve los datos con estado HTTP 200 OK
        }

        // Obtener un usuario por su ID
        [HttpGet("obtener/{categoriaId}")]
        public async Task<ActionResult<Categoria>> ObtenerPorId(int categoriaId)
        {
            var resultado = await _servicioCategoria.ObtenerPorIdAsync(categoriaId);

            if (resultado.TieneErrores)
            {
                return NotFound(resultado.Errores);  // Devuelve 404 si no se encuentra la familia
            }

            return Ok(resultado.Valor);  // Devuelve el usuario con estado 200 OK
        }

        // Crear un nuevo usuario
        [HttpPost("crear")]
        public async Task<ActionResult<FamiliaDTO>> Crear([FromBody] CrearFamiliaDTO familiaCreacionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var resultadoCreacion = await _servicioCategoria.CrearAsync(familiaCreacionDTO);

            //// En caso de que el usuario ya exista o haya un error, devolver BadRequest
            //if (resultadoCreacion.TieneErrores)
            //{
            //    return Conflict(resultadoCreacion.Errores);
            //}
            return Ok();
            //return Ok(new { id = resultadoCreacion.Valor.Id });
        }

        // Actualizar un usuario
        [HttpPut("actualizar/{categoriaId}")]
        public async Task<ActionResult<FamiliaDTO>> Actualizar(int categoriaId, [FromBody] ActualizarFamiliaDTO familiaActDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var resultadoActualizacion = await _servicioCategoria.ActualizarAsync(categoriaId, familiaActDTO);

            //if (resultadoActualizacion.TieneErrores)
            //{
            //    return NotFound(resultadoActualizacion.Errores);
            //}
            return Ok();
            //return Ok(resultadoActualizacion.Valor);  // Devuelve el usuario actualizado con estado 200 OK
        }

        // Eliminar un usuario
        [HttpDelete("eliminar/{categoriaId}")]
        public async Task<ActionResult> Eliminar(int categoriaId)
        {
            var resultadoEliminacion = await _servicioCategoria.EliminarAsync(categoriaId);

            if (resultadoEliminacion.TieneErrores)
            {
                return NotFound(resultadoEliminacion.Errores);
            }

            return NoContent();  // Devuelve 204 No Content si la eliminación fue exitosa
        }
    }
}
