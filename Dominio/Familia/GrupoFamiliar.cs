using Dominio.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Familia
{
    public class GrupoFamiliar
    {
        public int Id { get; set; }
        public string? Apellido { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaDeCreacion { get; set; }
        [Required]
        public Usuario UsuarioAdministrador { get; set; }
        public string UsuarioAdministradorId { get; set; }
        public List<MiembroFamilia> Miembros { get; set; }
        public bool Activo { get; set; }
        public GrupoFamiliar()
        {
            
        }
        public GrupoFamiliar(Usuario usuarioAdministrador, string apellido, string descripcion)
        {
            this.UsuarioAdministrador = usuarioAdministrador;
            this.UsuarioAdministradorId = usuarioAdministrador.Id;
            this.Apellido = apellido;
            this.Descripcion = descripcion;
            this.FechaDeCreacion = DateTime.Now;
            this.Miembros = new List<MiembroFamilia>() { new MiembroFamilia(usuarioAdministrador,this) };
            this.Activo = true;
        }

    }
}
