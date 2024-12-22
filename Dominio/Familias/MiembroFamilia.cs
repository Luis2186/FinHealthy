using Dominio.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Familias
{
    public class MiembroFamilia
    {
        public int Id { get; set; }
        public string? UsuarioId { get; set; }
        [Required (ErrorMessage ="El usuario es requerido, por favor asignelo")]
        public Usuario? Usuario { get; set; }
        public DateTime? FechaDeUnion { get; set; }
        public int? GrupoFamiliarId { get; set; }
        public Familia? GrupoFamiliar { get; set; }
        public bool Activo { get; set; }

        public MiembroFamilia()
        {
            
        }
        public MiembroFamilia(Usuario usuario,Familia grupoFamiliar)
        {
            this.Usuario = usuario;
            this.GrupoFamiliar = grupoFamiliar;
            this.FechaDeUnion = DateTime.Now;
            this.Activo = true;
        }

        public MiembroFamilia ConvertirUsuarioEnMiembro(Usuario usuario) {
            this.Usuario = usuario;
            this.Activo = true;
            return this;
        }

        public MiembroFamilia UnirserAGrupoFamiliar(Familia grupoFamiliar)
        {
            this.GrupoFamiliar = grupoFamiliar;
            GrupoFamiliarId = grupoFamiliar.Id;
            this.FechaDeUnion = DateTime.Now;
            return this;
        }






    }
}
