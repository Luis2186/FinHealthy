using AutoMapper;
using Dominio.Gastos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicio.DTOS.SubCategoriasDTO;
using Servicio.DTOS.CategoriasDTO;
using Servicio.S_Categorias;


namespace FinHealthAPI.Controllers
{
    /// <summary>
    /// Controlador para la gestión de categorías.
    /// </summary>
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
        [ProducesResponseType(typeof(IEnumerable<CategoriaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> ObtenerTodas(CancellationToken cancellationToken)
        {
            var resultado = await _servicioCategoria.ObtenerTodasLasCategorias(cancellationToken);

            if (resultado.TieneErrores)
                return NotFound(new ProblemDetails
                {
                    Title = "Error obtener categorias",
                    Detail = "Ah ocurrido un error al intentar obtener todas las categorias",
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });

            return Ok(resultado.Valor);
        }

        // Obtener un usuario por su ID
        [HttpGet("obtener/{categoriaId}")]
        [ProducesResponseType(typeof(CategoriaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoriaDTO>> ObtenerPorId(int categoriaId, CancellationToken cancellationToken)
        {
            var resultado = await _servicioCategoria.ObtenerCategoriaPorId(categoriaId, cancellationToken);

            if (resultado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error obtener categoria por id",
                    Detail = "Ah ocurrido un error al intentar al obtener la categoria por id",
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });
            }

            return Ok(resultado.Valor);
        }

        // Crear un nuevo usuario
        [HttpPost("crear")]
        [ProducesResponseType(typeof(CategoriaDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoriaDTO>> Crear([FromBody] CrearCategoriaDTO categoriaCreacionDTO, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Error de validacion",
                    Detail = "El cuerpo de la solicitud es invalido, contiene errores de validacion.",
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = ModelState.Keys
                        .SelectMany(key => ModelState[key].Errors.Select(error => new
                        {
                            Code = key,
                            Description = error.ErrorMessage
                        })) }
                });
            }

            var resultadoCreacion = await _servicioCategoria.CrearCategoria(categoriaCreacionDTO, cancellationToken);

            if (resultadoCreacion.TieneErrores)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Error al crear categoria",
                    Detail = "Ah ocurrido un error al intentar crear la categoria",
                    Status = StatusCodes.Status400BadRequest,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultadoCreacion.Errores }
                });
            }

            return CreatedAtAction(nameof(Crear), new { id = resultadoCreacion.Valor.Id }, resultadoCreacion.Valor);
        }

        // Actualizar un usuario
        [HttpPut("actualizar/{categoriaId}")]
        [ProducesResponseType(typeof(CategoriaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoriaDTO>> Actualizar(int categoriaId, [FromBody] ActualizarCategoriaDTO categoriaActDTO, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Error de validacion",
                    Detail = "El cuerpo de la solicitud es invalido, contiene errores de validacion.",
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = ModelState.Keys
                        .SelectMany(key => ModelState[key].Errors.Select(error => new
                        {
                            Code = key,
                            Description = error.ErrorMessage
                        })) }
                });
            }

            var resultadoActualizacion = await _servicioCategoria.ActualizarCategoria(categoriaId, categoriaActDTO, cancellationToken);

            if (resultadoActualizacion.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error al actualizar categoria",
                    Detail = "Ah ocurrido un error al intentar actualizar la categoria",
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultadoActualizacion.Errores }
                });
            }
            return Ok(resultadoActualizacion.Valor);
        }

        // Eliminar un usuario
        [HttpDelete("eliminar/{categoriaId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Eliminar(int categoriaId, CancellationToken cancellationToken)
        {
            var resultado = await _servicioCategoria.EliminarCategoria(categoriaId, cancellationToken);

            if (resultado.TieneErrores)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Error al eliminar categoria",
                    Detail = "Ah ocurrido un error al intentar eliminar la categoria",
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path,
                    Extensions = { ["errors"] = resultado.Errores }
                });
            }

            return NoContent();
        }

        // Obtener todos los usuarios con paginación
        [HttpGet("prueba")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> ObtenerPrueba()
        {
            var resultado = "Esto es una prueba de docker";

            return Ok(resultado);  // Devuelve los datos con estado HTTP 200 OK
        }


    }
}
