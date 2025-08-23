using AutoMapper;
using Dominio;
using Dominio.Usuarios;
using Servicio.DTOS.GastosDTO; // CrearGastoDTO
using Servicio.DTOS.GruposDTO; // GrupoDTO
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Servicio.S_Gastos;
using Servicio.Usuarios;
using System.Configuration;
using System.Net;

namespace FinHealthAPI.Controllers
{
    /// <summary>
    /// Controlador para la gestión de gastos.
    /// </summary>
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
        /// <summary>
        /// Crea un nuevo gasto.
        /// </summary>
        /// <param name="gastoCreacionDTO">Datos del gasto a crear.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Un objeto GrupoDTO con los detalles del grupo creado.</returns>
        /// <response code="200">Devuelve el ID del gasto creado.</response>
        /// <response code="400">Si hay errores de validación.</response>
        /// <response code="404">Si ya existe un gasto con ese ID.</response>
        [HttpPost("crear")]
        public async Task<ActionResult<GrupoDTO>> CrearGrupo([FromBody] CrearGastoDTO gastoCreacionDTO, CancellationToken cancellationToken)
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

            var resultado = await _servicioGasto.CrearGasto(gastoCreacionDTO, cancellationToken);

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
