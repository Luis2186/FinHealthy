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
        [ProducesResponseType(typeof(IEnumerable<SubCategoriaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<SubCategoriaDTO>>> ObtenerTodas(int grupoId, int categoriaId, CancellationToken cancellationToken)
        {
            var resultado = await _servicioSubCategoria.ObtenerSubCategorias(grupoId, categoriaId, cancellationToken);

            if (resultado.TieneErrores)
                return NotFound(new ProblemDetails
                {
                    Title = "Error obtener subcategorías",
                    Detail = "Ah ocurrido un error al intentar obtener todas las subcategorías",
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });

            return Ok(resultado.Valor);
        }

        // Obtener un usuario por su ID
        /// <summary>
        /// Obtiene una subcategoría por su ID.
        /// </summary>
        /// <param name="subCategoriaId">ID de la subcategoría.</param>
        /// <returns>Datos de la subcategoría.</returns>
        [HttpGet("obtener/{subCategoriaId}")]
        [ProducesResponseType(typeof(SubCategoriaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SubCategoriaDTO>> ObtenerPorId(int subCategoriaId, CancellationToken cancellationToken)
        {
            var resultado = await _servicioSubCategoria.ObtenerPorId(subCategoriaId, cancellationToken);

            if (resultado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error obtener subcategoría por id",
                    Detail = "Ah ocurrido un error al intentar obtener la subcategoría por id",
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });
            }

            return Ok(resultado.Valor);
        }

        // Crear un nuevo usuario
        /// <summary>
        /// Crea una nueva subcategoría.
        /// </summary>
        /// <param name="subCategoriaCreacionDTO">Datos de la subcategoría a crear.</param>
        /// <returns>Datos de la subcategoría creada.</returns>
        [HttpPost("crear")]
        [ProducesResponseType(typeof(SubCategoriaDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SubCategoriaDTO>> Crear([FromBody] CrearSubCategoriaDTO subCategoriaCreacionDTO, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Error de validación",
                    Detail = "El cuerpo de la solicitud es inválido, contiene errores de validación.",
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = ModelState.Keys
                        .SelectMany(key => ModelState[key].Errors.Select(error => new
                        {
                            Code = key,
                            Description = error.ErrorMessage
                        })) }
                });
            }

            var resultadoCreacion = await _servicioSubCategoria.Crear(subCategoriaCreacionDTO, cancellationToken);

            if (resultadoCreacion.TieneErrores)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Error al crear subcategoría",
                    Detail = "Ah ocurrido un error al intentar crear la subcategoría",
                    Status = StatusCodes.Status400BadRequest,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultadoCreacion.Errores }
                });
            }

            return CreatedAtAction(nameof(Crear), new { id = resultadoCreacion.Valor.Id }, resultadoCreacion.Valor);
        }

        // Actualizar un usuario
        /// <summary>
        /// Actualiza una subcategoría existente.
        /// </summary>
        /// <param name="categoriaId">ID de la subcategoría a actualizar.</param>
        /// <param name="subCategoriaActDTO">Datos actualizados de la subcategoría.</param>
        /// <returns>Datos de la subcategoría actualizada.</returns>
        [HttpPut("actualizar/{categoriaId}")]
        [ProducesResponseType(typeof(SubCategoriaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SubCategoriaDTO>> Actualizar(int categoriaId, [FromBody] ActualizarCategoriaDTO subCategoriaActDTO, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Error de validación",
                    Detail = "El cuerpo de la solicitud es inválido, contiene errores de validación.",
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = ModelState.Keys
                        .SelectMany(key => ModelState[key].Errors.Select(error => new
                        {
                            Code = key,
                            Description = error.ErrorMessage
                        })) }
                });
            }

            var resultadoActualizacion = await _servicioSubCategoria.Actualizar(categoriaId, subCategoriaActDTO, cancellationToken);

            if (resultadoActualizacion.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error al actualizar subcategoría",
                    Detail = "Ah ocurrido un error al intentar actualizar la subcategoría",
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultadoActualizacion.Errores }
                });
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Eliminar(int subCategoriaId, CancellationToken cancellationToken)
        {
            var resultado = await _servicioSubCategoria.Eliminar(subCategoriaId, cancellationToken);

            if (resultado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error al eliminar subcategoría",
                    Detail = "Ah ocurrido un error al intentar eliminar la subcategoría",
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });
            }

            return NoContent();
        }
    }
}
