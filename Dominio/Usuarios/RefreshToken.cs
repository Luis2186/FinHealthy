using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Usuarios
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string? UsuarioId { get; set; }
        public string? Token { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public bool Revocado { get; set; }

        public virtual Usuario? Usuario { get; set; }

        public RefreshToken()
        {

        }

        public RefreshToken(string Token, DateTime FechaExpiracion, string usuarioId)
        {
            this.FechaCreacion = DateTime.Now;
            this.Revocado = false;
            this.Token = Token;
            this.FechaExpiracion = FechaExpiracion;
            this.UsuarioId = usuarioId;
        }




    }
}
