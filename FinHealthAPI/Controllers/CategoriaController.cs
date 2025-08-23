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
        public async Task<ActionResult<CategoriaDTO>> ObtenerTodas()
        {
            var resultado = await _servicioCategoria.ObtenerTodasLasCategorias(HttpContext.RequestAborted);

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
        [HttpGet("obtener/{categoriaId}")]
        public async Task<ActionResult<CategoriaDTO>> ObtenerPorId(int categoriaId)
        {
            var resultado = await _servicioCategoria.ObtenerCategoriaPorId(categoriaId, HttpContext.RequestAborted);

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

            return Ok(resultado.Valor);  // Devuelve el usuario con estado 200 OK
        }

        // Crear un nuevo usuario
        [HttpPost("crear")]
        public async Task<ActionResult<CategoriaDTO>> Crear([FromBody] CrearCategoriaDTO categoriaCreacionDTO)
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

            var resultadoCreacion = await _servicioCategoria.CrearCategoria(categoriaCreacionDTO, HttpContext.RequestAborted);

            // En caso de que el usuario ya exista o haya un error, devolver BadRequest
            if (resultadoCreacion.TieneErrores)
            {
                return Conflict(resultadoCreacion.Errores);
            }

            return Ok(resultadoCreacion.Valor);
            //return Ok(new { id = resultadoCreacion.Valor.Id });
        }

        // Actualizar un usuario
        [HttpPut("actualizar/{categoriaId}")]
        public async Task<ActionResult<CategoriaDTO>> Actualizar(int categoriaId, [FromBody] ActualizarCategoriaDTO categoriaActDTO)
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

            var resultadoActualizacion = await _servicioCategoria.ActualizarCategoria(categoriaId, categoriaActDTO, HttpContext.RequestAborted);

            if (resultadoActualizacion.TieneErrores)
            {
                return NotFound(resultadoActualizacion.Errores);
            }
            return Ok(resultadoActualizacion.Valor);
            //return Ok(resultadoActualizacion.Valor);  // Devuelve el usuario actualizado con estado 200 OK
        }

        // Eliminar un usuario
        [HttpDelete("eliminar/{categoriaId}")]
        public async Task<ActionResult> Eliminar(int categoriaId)
        {
            var resultado = await _servicioCategoria.EliminarCategoria(categoriaId, HttpContext.RequestAborted);

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

        // Obtener todos los usuarios con paginación
        [HttpGet("prueba")]
        public async Task<ActionResult<Categoria>> ObtenerPrueba()
        {
            var resultado = "Esto es una prueba de docker";

            return Ok(resultado);  // Devuelve los datos con estado HTTP 200 OK
        }


    }
}
