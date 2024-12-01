using Dominio.Familias;
using Dominio.Usuarios;
using Servicio.DTOS.UsuariosDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.DTOS.MiembrosFamiliaDTO
{
    public class MiembroFamiliaDTO
    {
        public int Id { get; set; }
        public UsuarioDTO Usuario { get; set; }
        public DateTime? FechaDeUnion { get; set; }
        public bool Activo { get; set; }
    }
}
