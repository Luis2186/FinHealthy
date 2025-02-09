using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.DTOS.GrupoDTO
{
    public class UnirseAGrupoDTO
    {
        [Required(ErrorMessage ="El usuario es requerido,por favor ingreselo")]
        public string? UsuarioId { get; set; }
        [Required(ErrorMessage = "El grupo es requerido,por favor ingreselo")]
        public int GrupoGastoId { get; set; }
        [Required(ErrorMessage = "El codigo es requerido, por favor ingreselo")]
        public string? Codigo { get; set; }

    }
}
