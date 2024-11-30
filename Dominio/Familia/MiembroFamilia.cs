using Dominio.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Familia
{
    public class MiembroFamilia
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public DateTime FechaDeUnion { get; set; }
        public int GrupoFamiliarId { get; set; }
        public GrupoFamiliar GrupoFamiliar { get; set; }
        public bool Activo { get; set; }

        public MiembroFamilia()
        {
            
        }
        public MiembroFamilia(Usuario usuario,GrupoFamiliar grupoFamiliar)
        {
            this.Usuario = usuario;
            this.FechaDeUnion = DateTime.Now;
            this.GrupoFamiliar = grupoFamiliar;
            this.FechaDeUnion = DateTime.Now;
            this.Activo = true;
        }










    }
}
