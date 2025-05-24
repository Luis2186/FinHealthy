using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicio.DTOS.GastosDTO;
using Servicio.DTOS.GruposDTO;
using Servicio.S_Categorias;
using Servicio.S_Gastos;

namespace FinHealthAPI.Controllers
{
    [Authorize(Roles = "Sys_Adm , Administrador, Usuario")]
    [ApiController]
    [Route("/gasto")]
    public class GastoController : Controller
    {
        private readonly IServicioGasto _servicioGasto;
        private readonly IMapper _mapper;

        public GastoController(IServicioGasto servicioGasto, IMapper mapper)
        {
            _servicioGasto = servicioGasto;
            _mapper = mapper;
        }


        // Crear un nuevo usuario
        [HttpPost("crear")]
        public async Task<ActionResult<GrupoDTO>> CrearGrupo([FromBody] CrearGastoDTO gastoCreacionDTO)
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

            var resultado = await _servicioGasto.CrearGasto(gastoCreacionDTO);

            // En caso de que el usuario ya exista o haya un error, devolver BadRequest
            if (resultado.TieneErrores)
            {
                return Conflict(new ProblemDetails
                {
                    Title = "Error al ingresar el gasto",
                    Detail = "Ah ocurrido un error al intentar crear el grupo",
                    Status = 404,
                    Instance = HttpContext.Request.Path,
                    Extensions = {
                        ["errors"] = resultado.Errores
                    }
                });
            }

            return Ok(new { gastoId = resultado.Valor });
        }
    }
}
