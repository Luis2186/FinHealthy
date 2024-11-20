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
        public string? Token { get; set; }
    }
}
