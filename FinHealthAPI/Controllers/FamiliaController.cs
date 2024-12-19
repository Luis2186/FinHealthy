using AutoMapper;
using Dominio;
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

            if (resultado.TieneErrores) return NotFound(new ProblemDetails
            {
                Title = "Error al obtener todas las familias",
                Detail = "Ah ocurrido un error al intentar obtener todas las familias",
                Status = 404,
                Instance = HttpContext.Request.Path,
                Extensions = {
                        ["errors"] = resultado.Errores
                    }
            });

            return Ok(resultado.Valor);  // Devuelve los datos con estado HTTP 200 OK
        }

        // Obtener un usuario por su ID
        [HttpGet("obtener/{familiaId}")]
        public async Task<ActionResult<FamiliaDTO>> ObtenerFamiliaPorId(int familiaId)
        {
            var resultado = await _servicioFamilia.ObtenerFamiliaPorId(familiaId);

            if (resultado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error al obtener la familia por id",
                    Detail = "Ah ocurrido un error al intentar obtener la familia por id",
                    Status = 404,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = resultado.Errores
                    }
                });  // Devuelve 404 si no se encuentra la familia
            }

            return Ok(resultado.Valor);  // Devuelve el usuario con estado 200 OK
        }

        // Crear un nuevo usuario
        [HttpPost("crear")]
        public async Task<ActionResult<FamiliaDTO>> CrearFamilia([FromBody] CrearFamiliaDTO familiaCreacionDTO)
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
                        ["errors"] = ModelState.Keys.ToDictionary(
                            key => key,
                            key => ModelState[key].Errors.Select(e => e.ErrorMessage).ToArray())
                    }
                });
            }

            var resultado = await _servicioFamilia.CrearFamilia(familiaCreacionDTO);

            // En caso de que el usuario ya exista o haya un error, devolver BadRequest
            if (resultado.TieneErrores)
            {
                return Conflict(new ProblemDetails
                {
                    Title = "Error al crear la familia",
                    Detail = "Ah ocurrido un error al intentar crear la familia",
                    Status = 404,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = resultado.Errores
                    }
                });
            }

            return Ok(new { familiaId = resultado.Valor.Id });
        }

        // Actualizar un usuario
        [HttpPut("actualizar/{familiaId}")]
        public async Task<ActionResult<FamiliaDTO>> ActualizarFamilia(int familiaId, [FromBody] ActualizarFamiliaDTO familiaActDTO)
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
                        ["errors"] = ModelState.Keys.ToDictionary(
                            key => key,
                            key => ModelState[key].Errors.Select(e => e.ErrorMessage).ToArray())
                    }
                });
            }

            var resultado = await _servicioFamilia.ActualizarFamilia(familiaId, familiaActDTO);

            if (resultado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error al actualiza la familia",
                    Detail = "Ah ocurrido un error al intentar actualizar la familia",
                    Status = 404,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = resultado.Errores
                    }
                });
            }

            return Ok(resultado.Valor);  // Devuelve el usuario actualizado con estado 200 OK
        }

        // Eliminar un usuario
        [HttpDelete("eliminar/{familiaId}")]
        public async Task<ActionResult> EliminarUsuario(int familiaId)
        {
            var resultado = await _servicioFamilia.EliminarFamilia(familiaId);

            if (resultado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error al eliminar familia",
                    Detail = "Ah ocurrido un error al intentar eliminar la familia",
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
