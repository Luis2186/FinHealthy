using AutoMapper;
using Dominio.Familias;
using Dominio.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicio.DTOS.FamiliasDTO;
using Servicio.DTOS.SolicitudesDTO;
using Servicio.DTOS.UsuariosDTO;
using Servicio.Notificaciones.NotificacionesDTO;
using Servicio.Pdf;
using Servicio.S_Familias;

namespace FinHealthAPI.Controllers
{
    [Authorize(Roles = "Sys_Adm , Administrador, Usuario")]
    [ApiController]
    [Route("/solicitud")]
    public class SolicitudController : Controller
    {
        private readonly IServicioFamilia _servicioFamilia;
        private readonly IMapper _mapper;

        public SolicitudController(IServicioFamilia servicioFamilia, IMapper mapper)
        {
            _servicioFamilia = servicioFamilia;
            _mapper = mapper;
        }


        // Obtener todos los usuarios con paginación
        [HttpGet("porAdmin")]
        public async Task<ActionResult<SolicitudDTO>> ObtenerSolicitudesPorAdministrador([FromBody] PaginacionSolicitudDTO solicitudes)
        {
            var resultado = await _servicioFamilia.ObtenerSolicitudesPorAdministrador(solicitudes.IdAdministrador,solicitudes.Estado);

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
        [HttpPost("enviar")]
        public async Task<ActionResult<FamiliaDTO>> EnviarSolicitud([FromBody] EnviarSolicitudDTO enviarSolicitudDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var solicitudEnviada = await _servicioFamilia.EnviarSolicitudIngresoAFamilia(enviarSolicitudDTO);

            // En caso de que el usuario ya exista o haya un error, devolver BadRequest
            if (solicitudEnviada.TieneErrores)
            {
                return Conflict(solicitudEnviada.Errores);
            }

            return Ok(new { solicitudId = solicitudEnviada.Valor.Id });
        }

        // Crear un nuevo usuario
        [HttpPost("aceptar/{idSolicitud}")]
        public async Task<ActionResult<bool>> AceptarSolicitudDeUnionFamilia(int idSolicitud)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var unionFamilia = await _servicioFamilia.AceptarSolicitudIngresoAFamilia(idSolicitud);

            // En caso de que el usuario ya exista o haya un error, devolver BadRequest
            if (unionFamilia.TieneErrores)
            {
                return Conflict(unionFamilia.Errores);
            }

            return NoContent(); 
        }

        // Crear un nuevo usuario
        [HttpPost("porCodigo")]
        public async Task<ActionResult<bool>> UnirseConCodigoAFamilia(UnirseAFamiliaDTO solicitud)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var unionFamilia = await _servicioFamilia.IngresoAFamiliaConCodigo(solicitud);

            // En caso de que el usuario ya exista o haya un error, devolver BadRequest
            if (unionFamilia.TieneErrores)
            {
                return Conflict(unionFamilia.Errores);
            }

            return NoContent();
        }
    }
}
