using Dominio.Usuarios;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinHealthAPI.Authentication
{
    internal sealed class ProvedorJwt : IProvedorJwt
    {
        private readonly JwtOpciones _jwtOpciones;

        public ProvedorJwt(IOptions<JwtOpciones> jwtOpciones)
        {
            _jwtOpciones=jwtOpciones.Value;
        }

        public string Generate(Usuario usuario)
        {
            var claims = new Claim[]
            {
                new (JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new (JwtRegisteredClaimNames.UniqueName, usuario.UserName.ToString()),
                new (JwtRegisteredClaimNames.Email, usuario.Email.ToString())
            };

            var credencialesDeInicio = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtOpciones.ClaveSecreta)),
                    SecurityAlgorithms.HmacSha256
                );

            var token = new JwtSecurityToken(
                _jwtOpciones.Editor,
                _jwtOpciones.Audiencia,
                claims,
                null,
                DateTime.UtcNow.AddHours(2),
                credencialesDeInicio
                );

            string tokenValue = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return tokenValue;

        }
    }
}
