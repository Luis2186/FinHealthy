using Dominio.Grupos;
using Dominio.Notificaciones;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Usuarios
{
    public class Usuario : IdentityUser
    {
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public DateTime FechaDeRegistro { get; set; }
        public DateTime FechaDeNacimiento { get; set; }
        public int Edad { get; set; }
        public bool Activo { get; set; }
        [NotMapped]
        public List<string> Roles { get; set; } = new List<string>();
        [NotMapped]
        public string? Token { get; set; }
        [NotMapped]
        public string? RefreshToken { get; set; }
        // Relación con las notificaciones recibidas
        public DateTime? FechaDeUnion { get; set; }
        public int? GrupoDeGastosId { get; set; }
        public List<Grupo>? GrupoDeGastos { get; set; } = new List<Grupo>();

        public ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();
        

        public Usuario UnirseAGrupo(Grupo grupoGastos)
        {
            if (!GrupoDeGastos.Contains(grupoGastos)) GrupoDeGastos.Add(grupoGastos);
            grupoGastos.MiembrosGrupoGasto.Add(this);
            this.FechaDeUnion = DateTime.Now;
            return this;
        }
        public Usuario DejarGrupo(Grupo grupoGastos)
        {
            if (!GrupoDeGastos.Contains(grupoGastos))
            {
                throw new InvalidOperationException("El usuario no pertenece a este grupo.");
            }

            GrupoDeGastos.Remove(grupoGastos);
            grupoGastos.MiembrosGrupoGasto.Remove(this);  // Eliminar del grupo
            return this;
        }
        public void AsignarRoles(List<string> Roles)
        {
            this.Roles = Roles;
        }

        public void AsignarToken(string token,string refreshToken)
        {
            this.Token = token; 
            this.RefreshToken = refreshToken;
        }


    }
}
