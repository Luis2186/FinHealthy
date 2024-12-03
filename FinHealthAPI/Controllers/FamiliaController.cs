using AutoMapper;
using Dominio.Familias;
using Dominio.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicio.DTOS.FamiliasDTO;
using Servicio.DTOS.UsuariosDTO;
using Servicio.Notificaciones.NotificacionesDTO;
using Servicio.Pdf;
using Servicio.S_Familias;

namespace FinHealthAPI.Controllers
{
    [Authorize(Roles = "Sys_Adm , Administrador, Usuario")]
    [ApiController]
    [Route("/familia")]
    public class FamiliaController : Controller
    {
        private readonly IServicioFamilia _servicioFamilia;
        private readonly IMapper _mapper;

        public FamiliaController(IServicioFamilia servicioFamilia, IMapper mapper)
        {
            _servicioFamilia = servicioFamilia;
            _mapper = mapper;
        }


        // Obtener todos los usuarios con paginación
        [HttpGet("todas")]
        public async Task<ActionResult<Familia>> ObtenerFamilias()
        {
            var resultado = await _servicioFamilia.ObtenerTodasLasFamilias();

            if (resultado.TieneErrores) return NotFound(resultado.Errores);

            return Ok(resultado.Valor);  // Devuelve los datos con estado HTTP 200 OK
        }

        // Obtener un usuario por su ID
        [HttpGet("obtener/{familiaId}")]
        public async Task<ActionResult<FamiliaDTO>> ObtenerUsuarioPorId(int familiaId)
        {
            var familia = await _servicioFamilia.ObtenerFamiliaPorId(familiaId);

            if (familia.TieneErrores)
            {
                return NotFound(familia.Errores);  // Devuelve 404 si no se encuentra la familia
            }

            return Ok(familia.Valor);  // Devuelve el usuario con estado 200 OK
        }

        // Crear un nuevo usuario
        [HttpPost("crear")]
        public async Task<ActionResult<FamiliaDTO>> CrearFamilia([FromBody] CrearFamiliaDTO familiaCreacionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var familiaCreada = await _servicioFamilia.CrearFamilia(familiaCreacionDTO);

            // En caso de que el usuario ya exista o haya un error, devolver BadRequest
            if (familiaCreada.TieneErrores)
            {
                return Conflict(familiaCreada.Errores);
            }

            return Ok(new { familiaId = familiaCreada.Valor.Id });
        }

        // Actualizar un usuario
        [HttpPut("actualizar/{familiaId}")]
        public async Task<ActionResult<FamiliaDTO>> ActualizarFamilia(int familiaId, [FromBody] ActualizarFamiliaDTO familiaActDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var familiaActualizada = await _servicioFamilia.ActualizarFamilia(familiaId, familiaActDTO);

            if (familiaActualizada.TieneErrores)
            {
                return NotFound(familiaActualizada.Errores);
            }

            return Ok(familiaActualizada.Valor);  // Devuelve el usuario actualizado con estado 200 OK
        }

        // Eliminar un usuario
        [HttpDelete("eliminar/{familiaId}")]
        public async Task<ActionResult> EliminarUsuario(int familiaId)
        {
            var familiaEliminada = await _servicioFamilia.EliminarFamilia(familiaId);

            if (familiaEliminada.TieneErrores)
            {
                return NotFound(familiaEliminada.Errores);
            }

            return NoContent();  // Devuelve 204 No Content si la eliminación fue exitosa
        }


    }
}
