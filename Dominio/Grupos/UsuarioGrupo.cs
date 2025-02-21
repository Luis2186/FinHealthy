using Dominio.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Grupos
{
    public class UsuarioGrupo
    {
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int GrupoId { get; set; }
        public Grupo Grupo { get; set; }

        // Propiedades adicionales
        public DateTime FechaDeUnion { get; set; }
    }
}
