using Servicio.DTOS.MiembrosFamiliaDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.DTOS.SolicitudesDTO
{
    public class PaginacionSolicitudDTO
    {
        public IEnumerable<PaginacionSolicitudDTO>? Solicitudes { get; set; }

        [Required(ErrorMessage = "El estado de las solicitudes es requerido")]
        [RegularExpression("^(Pendiente|Aceptada|Rechazada)$", ErrorMessage = "El estado debe ser 'Pendiente', 'Aceptada' o 'Rechazada'.")]
        public string? Estado { get; set; }
        [Required (ErrorMessage ="El id del administrador es requerido")]
        public string? IdAdministrador { get; set; }
        public int TotalElementos { get; set; }
        public int TotalPaginas { get; set; }
        public int PaginaActual { get; set; }
        public int ElementosPorPagina { get; set; }
    }
}
