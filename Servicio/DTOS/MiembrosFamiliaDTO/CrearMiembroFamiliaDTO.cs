using Dominio.Familias;
using Dominio.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.DTOS.MiembrosFamiliaDTO
{
    public class CrearMiembroFamiliaDTO : ActualizarMiembroFamiliaDTO
    {
        [Required(ErrorMessage ="El id del usuario es requerido para crear el miembro, por favor ingreselo")]
        public string UsuarioId { get; set; }
        public string? CodigoAcceso { get; set; }
    }
}
