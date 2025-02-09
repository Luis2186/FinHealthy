using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.DTOS.GruposDTO
{
    public class ActualizarGrupoDTO
    {
        [Required(ErrorMessage = "El nombre del grupo es un campo requerido, por favor ingreselo")]
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public bool Activo { get; set; }
        //public List<UsuarioDTO> Miembros { get; set; }

  
    }
}
