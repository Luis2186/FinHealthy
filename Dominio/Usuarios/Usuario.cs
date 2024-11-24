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
        public DateTime? FechaDeRegistro { get; set; }
        public DateTime? FechaDeNacimiento { get; set; }
        public int Edad { get; set; }
        public bool Activo { get; set; }
        [NotMapped]
        public List<string> Roles { get; set; }
        [NotMapped]
        public string? Token { get; set; }
        // Relación con las notificaciones recibidas
        public ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();
    

    public void AsignarRoles(List<string> Roles)
        {
            this.Roles = Roles;
        }

        public void AsignarToken(string token)
        {
            this.Token = token; 
        }

    }
}
