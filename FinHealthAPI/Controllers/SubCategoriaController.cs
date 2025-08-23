using AutoMapper;
using Dominio.Gastos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicio.DTOS.SubCategoriasDTO;
using Servicio.DTOS.CategoriasDTO;
using Servicio.S_Categorias;
using Servicio.S_Categorias.S_SubCategorias;

namespace FinHealthAPI.Controllers
{
    /// <summary>
    /// Controlador para la gestión de subcategorías.
    /// </summary>
    [Authorize(Roles = "Sys_Adm , Administrador, Usuario")]
    [ApiController]
    [Route("/subcategoria")]
    public class SubCategoriaController : ControllerBase
    {
        private readonly IServicioSubCategoria _servicioSubCategoria;
        private readonly IMapper _mapper;

        public SubCategoriaController(IServicioSubCategoria servicioSubCategoria, IMapper mapper)
        {
            _servicioSubCategoria = servicioSubCategoria;
            _mapper = mapper;
        }


        // Obtener todos los usuarios con paginación
        /// <summary>
        /// Obtiene todas las subcategorías de una categoría específica en un grupo.
        /// </summary>
        /// <param name="grupoId">ID del grupo.</param>
        /// <param name="categoriaId">ID de la categoría.</param>
        /// <returns>Lista de subcategorías.</returns>
        [HttpGet("todas/{grupoId}/{categoriaId}")]
        public async Task<ActionResult<SubCategoriaDTO>> ObtenerTodas(int grupoId, int categoriaId)
        {
            var resultado = await _servicioSubCategoria.ObtenerSubCategorias(grupoId, categoriaId, HttpContext.RequestAborted);

            if (resultado.TieneErrores) return NotFound(
                new ProblemDetails
                {
                    Title = "Error obtener categorias",
                    Detail = "Ah ocurrido un error al intentar obtener todas las categorias",
                    Status = 404,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = resultado.Errores
                }
                });

            return Ok(resultado.Valor);  // Devuelve los datos con estado HTTP 200 OK
        }

        // Obtener un usuario por su ID
        /// <summary>
        /// Obtiene una subcategoría por su ID.
        /// </summary>
        /// <param name="subCategoriaId">ID de la subcategoría.</param>
        /// <returns>Datos de la subcategoría.</returns>
        [HttpGet("obtener/{subCategoriaId}")]
        public async Task<ActionResult<SubCategoriaDTO>> ObtenerPorId(int subCategoriaId)
        {
            var resultado = await _servicioSubCategoria.ObtenerPorId(subCategoriaId, HttpContext.RequestAborted);

            if (resultado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error obtener categoria por id",
                    Detail = "Ah ocurrido un error al intentar al obtener la categoria por id",
                    Status = 404,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = resultado.Errores
                }
                });
            }

            return Ok(resultado.Valor);  // Devuelve el usuario with estado 200 OK
        }

        // Crear un nuevo usuario
        /// <summary>
        /// Crea una nueva subcategoría.
        /// </summary>
        /// <param name="subCategoriaCreacionDTO">Datos de la subcategoría a crear.</param>
        /// <returns>Datos de la subcategoría creada.</returns>
        [HttpPost("crear")]
        public async Task<ActionResult<SubCategoriaDTO>> Crear([FromBody] CrearSubCategoriaDTO subCategoriaCreacionDTO)
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
                                Code = key,
                                Description = error.ErrorMessage
                            }))
                    }
                });
            }

            var resultadoCreacion = await _servicioSubCategoria.Crear(subCategoriaCreacionDTO, HttpContext.RequestAborted);

            if (resultadoCreacion.TieneErrores)
            {
                return Conflict(resultadoCreacion.Errores);
            }

            return Ok(resultadoCreacion.Valor);
        }

        // Actualizar un usuario
        /// <summary>
        /// Actualiza una subcategoría existente.
        /// </summary>
        /// <param name="categoriaId">ID de la subcategoría a actualizar.</param>
        /// <param name="subCategoriaActDTO">Datos actualizados de la subcategoría.</param>
        /// <returns>Datos de la subcategoría actualizada.</returns>
        [HttpPut("actualizar/{categoriaId}")]
        public async Task<ActionResult<SubCategoriaDTO>> Actualizar(int categoriaId, [FromBody] ActualizarCategoriaDTO subCategoriaActDTO)
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
                                Code = key,
                                Description = error.ErrorMessage
                            }))
                    }
                });
            }

            var resultadoActualizacion = await _servicioSubCategoria.Actualizar(categoriaId, subCategoriaActDTO, HttpContext.RequestAborted);

            if (resultadoActualizacion.TieneErrores)
            {
                return NotFound(resultadoActualizacion.Errores);
            }
            return Ok(resultadoActualizacion.Valor);
        }

        // Eliminar un usuario
        /// <summary>
        /// Elimina una subcategoría por su ID.
        /// </summary>
        /// <param name="subCategoriaId">ID de la subcategoría a eliminar.</param>
        /// <returns>Respuesta vacía si la eliminación fue exitosa.</returns>
        [HttpDelete("eliminar/{subCategoriaId}")]
        public async Task<ActionResult> Eliminar(int subCategoriaId)
        {
            var resultado = await _servicioSubCategoria.Eliminar(subCategoriaId, HttpContext.RequestAborted);

            if (resultado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error al eliminar categoria",
                    Detail = "Ah ocurrido un error al intentar eliminar la categoria",
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
