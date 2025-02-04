using AutoMapper;
using Dominio;
using Dominio.Grupos;
using Dominio.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicio.DTOS.GrupoDTO;
using Servicio.DTOS.UsuariosDTO;
using Servicio.Notificaciones.NotificacionesDTO;
using Servicio.Pdf;
using Servicio.S_Grupos;

namespace FinHealthAPI.Controllers
{
    [Authorize(Roles = "Sys_Adm , Administrador, Usuario")]
    [ApiController]
    [Route("/grupo")]
    public class GrupoController : Controller
    {
        private readonly IServicioGrupos _servicioGastos;
        private readonly IMapper _mapper;

        public GrupoController(IServicioGrupos servicioGastos, IMapper mapper)
        {
            _servicioGastos = servicioGastos;
            _mapper = mapper;
        }


        // Obtener todos los usuarios con paginación
        [HttpGet("todas")]
        public async Task<ActionResult<Grupo>> ObtenerGrupos()
        {
            var resultado = await _servicioGastos.ObtenerTodosLosGrupos();

            if (resultado.TieneErrores) return NotFound(new ProblemDetails
            {
                Title = "Error al obtener todos los grupos",
                Detail = "Ah ocurrido un error al intentar obtener todos los grupos",
                Status = 404,
                Instance = HttpContext.Request.Path,
                Extensions = {
                        ["errors"] = resultado.Errores
                    }
            });

            return Ok(resultado.Valor);  // Devuelve los datos con estado HTTP 200 OK
        }

        // Obtener un usuario por su ID
        [HttpGet("obtener/{grupoId}")]
        public async Task<ActionResult<GrupoDTO>> ObtenerFamiliaPorId(int grupoId)
        {
            var resultado = await _servicioGastos.ObtenerGrupoPorId(grupoId);

            if (resultado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error al obtener el grupo por id",
                    Detail = "Ah ocurrido un error al intentar obtener el grupo por id",
                    Status = 404,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = resultado.Errores
                    }
                });  // Devuelve 404 si no se encuentra la grupo
            }

            return Ok(resultado.Valor);  // Devuelve el usuario con estado 200 OK
        }

        // Crear un nuevo usuario
        [HttpPost("crear")]
        public async Task<ActionResult<GrupoDTO>> CrearFamilia([FromBody] CrearGrupoDTO grupoCreacionDTO)
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
                      ["errors"] = ModelState.Keys
                            .SelectMany(key => ModelState[key].Errors.Select(error => new
                            {
                                Code = key, // Aquí puedes ajustar el código como desees
                                Description = error.ErrorMessage
                            }))
                    }
                });
            }

            var resultado = await _servicioGastos.CrearGrupo(grupoCreacionDTO);

            // En caso de que el usuario ya exista o haya un error, devolver BadRequest
            if (resultado.TieneErrores)
            {
                return Conflict(new ProblemDetails
                {
                    Title = "Error al crear el grupo",
                    Detail = "Ah ocurrido un error al intentar crear el grupo",
                    Status = 404,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = resultado.Errores
                    }
                });
            }

            return Ok(new { grupoId = resultado.Valor.Id });
        }

        // Actualizar un usuario
        [HttpPut("actualizar/{grupoId}")]
        public async Task<ActionResult<GrupoDTO>> ActualizarFamilia(int grupoId, [FromBody] ActualizarGrupoDTO grupoActDTO)
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
                       ["errors"] = ModelState.Keys
                            .SelectMany(key => ModelState[key].Errors.Select(error => new
                            {
                                Code = key, // Aquí puedes ajustar el código como desees
                                Description = error.ErrorMessage
                            }))
                    }
                });
            }

            var resultado = await _servicioGastos.ActualizarGrupo(grupoId, grupoActDTO);

            if (resultado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error al actualizar el grupo",
                    Detail = "Ah ocurrido un error al intentar actualizar el grupo",
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
        [HttpDelete("eliminar/{grupoId}")]
        public async Task<ActionResult> EliminarFamilia(int grupoId)
        {
            var resultado = await _servicioGastos.EliminarGrupo(grupoId);

            if (resultado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error al eliminar grupo",
                    Detail = "Ah ocurrido un error al intentar eliminar el grupo",
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
