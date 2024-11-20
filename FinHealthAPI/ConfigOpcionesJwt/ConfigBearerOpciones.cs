using FinHealthAPI.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FinHealthAPI.ConfigOpcionesJwt
{
    public class ConfigBearerOpciones : IConfigureOptions<JwtBearerOptions>
    {
        private readonly JwtOpciones _jwtOpciones;

        public ConfigBearerOpciones(IOptions<JwtOpciones> jwtOpciones)
        {
            _jwtOpciones = jwtOpciones.Value;
        }

        public void Configure( JwtBearerOptions options)
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtOpciones.Editor,
                ValidAudience = _jwtOpciones.Audiencia,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOpciones.ClaveSecreta))
            };
        }
    }
}
