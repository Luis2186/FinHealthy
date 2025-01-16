using Dominio.Usuarios;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositorio.Repositorios.Token;
using Repositorio.Repositorios.Usuarios;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Servicio.Authentication
{
    public sealed class ProveedorToken(IConfiguration configuration, IRepositorioRefreshToken _repoRefreshToken,
        IRepositorioUsuario _repoUsuario)
    {
        public async Task<(string accessToken, string refreshToken)> GenerarTokens(Usuario usuario,RefreshToken refreshTokenAnterior) { 
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

            var resultadoRoles = await _repoUsuario.ObtenerRolesPorUsuario(usuario.Id);
            var roles = resultadoRoles.Valor.ToList();
            usuario.AsignarRoles(roles);

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

            var manejador = new JwtSecurityTokenHandler();
            var token = manejador.CreateToken(descripcionToken);
            string accessToken = manejador.WriteToken(token);

            string refreshTokenStr = Guid.NewGuid().ToString();
            var expiracion_RefreshToken = DateTime.UtcNow.AddMinutes(expiracionRefreshToken);
            RefreshToken refreshTokenNuevo = new RefreshToken(refreshTokenStr, expiracion_RefreshToken, usuario.Id);

            if(refreshTokenAnterior == null)
            {
                var resultado = await _repoRefreshToken.CrearAsync(refreshTokenNuevo);
            }else
            {
                var resultado = await _repoRefreshToken.RevocarYCrearNuevo(refreshTokenAnterior, refreshTokenNuevo);
            }

            return (accessToken, refreshTokenStr);
        }

 

    }
}
