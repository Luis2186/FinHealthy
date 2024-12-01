using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.DTOS.FamiliasDTO
{
    public class ActualizarFamiliaDTO
    {
        [Required(ErrorMessage = "El apellido de la familia es un campo requerido, por favor ingreselo")]
        public string? Apellido { get; set; }
        public string? Descripcion { get; set; }
        public bool Activo { get; set; }
        //public List<MiembroFamilia> Miembros { get; set; }
    }
}
