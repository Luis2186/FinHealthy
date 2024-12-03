using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.DTOS.FamiliasDTO
{
    public class UnirseAFamiliaDTO
    {
        [Required(ErrorMessage ="El usuario es requerido,por favor ingreselo")]
        public string? UsuarioId { get; set; }
        [Required(ErrorMessage = "La familia es requerida,por favor ingresela")]
        public int FamiliaId { get; set; }


    }
}
