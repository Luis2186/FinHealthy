using Dominio.Familias;
using Dominio.Usuarios;
using Servicio.DTOS.MiembrosFamiliaDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.DTOS.FamiliasDTO
{
    public class FamiliaDTO
    {
        public int Id { get; set; }
        public string? Apellido { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaDeCreacion { get; set; }
        public string UsuarioAdministradorId { get; set; }
        public List<MiembroFamiliaDTO> Miembros { get; set; }
        public bool Activo { get; set; }
    }
}
