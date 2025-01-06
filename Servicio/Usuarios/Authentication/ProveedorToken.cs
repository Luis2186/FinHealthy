using Dominio.Usuarios;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Servicio.Authentication
{
    public sealed class ProveedorToken(IConfiguration configuration)
    {
        public string Crear(Usuario usuario,List<string> roles) { 
            string claveSecreta = configuration["Jwt:ClaveSecreta"];
            string audiencia = configuration["Jwt:Audiencia"];
            string editor = configuration["Jwt:Editor"];
            int minutosDeExpiracion = configuration.GetValue<int>("Jwt:ExpiracionAccessToken");
            int expiracionRefreshToken = configuration.GetValue<int>("Jwt:ExpiracionRefreshToken");
             
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.UserName.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email.ToString()),
             };

            // Agregar roles como claims
            foreach (var role in roles)
            {
                claims.Add(new Claim("roles", role));
            }

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(claveSecreta));
            var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddMinutes(minutosDeExpiracion);
            var descripcionToken = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiracion,
                SigningCredentials = credenciales,
                Issuer = editor,
                Audience = audiencia,
            };

            var manejador = new JsonWebTokenHandler();

            string token = manejador.CreateToken(descripcionToken);

            return token;
        }
    
    }
}
