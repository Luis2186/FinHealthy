using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using Repositorio.Repositorios.Token;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinHealthAPI.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TokenValidationMiddleware> _logger;
        private readonly IConfiguration configuration;

        public TokenValidationMiddleware(RequestDelegate next,
            ILogger<TokenValidationMiddleware> logger, IConfiguration _config )
        {
            _next = next;
            _logger = logger;
            configuration = _config;         
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Intentar obtener el token del header o cookies
            var (accessToken, refreshToken) = GetTokensFromRequest(context);

            // Si no hay token, simplemente continua con la solicitud
            if (string.IsNullOrEmpty(accessToken) && string.IsNullOrEmpty(refreshToken))
            {
                await _next(context);
                return;
            }

            ClaimsPrincipal principal = null;

            try
            {
                // Intentar validar el accessToken
                if (!string.IsNullOrEmpty(accessToken))
                {
                    principal = ValidateToken(accessToken);
                }
            }
            catch (Exception ex)
            {
                // Si la validación falla, respondemos con Unauthorized
                _logger.LogError("Token no válido: " + ex.Message);
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Unauthorized: Invalid token");
                return;
            }

            // Si el token es válido, lo agregamos al contexto
            if (principal != null)
            {
                context.Items["User"] = principal;
            }
            // Continuamos con el pipeline de la solicitud
            await _next(context);
        }

        // Método para extraer el token del header o cookies

    private (string accessToken, string refreshToken) GetTokensFromRequest(HttpContext context)
    {
        string accessToken = context.Request.Cookies["access_token"];
        string refreshToken = context.Request.Cookies["refresh_token"];

        return (accessToken, refreshToken);
    }

        // Método para validar el token JWT
        private ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Tu clave secreta para validar la firma
            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:ClaveSecreta"]));

            // Obtener la audiencia y el emisor desde la configuración
            string audiencia = configuration["Jwt:Audiencia"];
            string editor = configuration["Jwt:Editor"];

            var validationParameters = new TokenValidationParameters
            {
                // Establecemos las validaciones necesarias
                ValidateIssuer = true,  // Validamos que el emisor sea el esperado
                ValidateAudience = true,  // Validamos que la audiencia sea la esperada
                ValidateLifetime = true,  // Validamos que el token no haya expirado
                ValidateIssuerSigningKey = true,  // Validamos la firma con la clave secreta

                // Definimos los valores válidos
                ValidIssuer = editor,  // El emisor debe coincidir con lo que está configurado
                ValidAudience = audiencia,  // La audiencia debe coincidir con lo que está configurado
                IssuerSigningKey = llave  // La clave secreta para validar la firma
            };

            try
            {
                // Validamos el token con los parámetros establecidos
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;  // Si el token es válido, retornamos el ClaimsPrincipal
            }
            catch (SecurityTokenException ex)
            {
                // Si ocurre un error en la validación (token inválido, expirado, etc.), manejamos la excepción
                throw new UnauthorizedAccessException("Token inválido", ex);
            }
        }

        // Método para renovar el accessToken usando el refreshToken
        //private async Task<string> RenovarAccessToken(string refreshToken, HttpContext context)
        //{
        //    // Validar el refreshToken en la base de datos o caché
        //    var refreshTokenValido = await _repositorioRefreshToken.ValidarRefreshTokenAsync(refreshToken);

        //    if (!refreshTokenValido)
        //    {
        //        throw new SecurityTokenException("Refresh token inválido o expirado");
        //    }

        //    // Obtener el usuario asociado al refreshToken
        //    var usuario = await _repositorioUsuario.ObtenerUsuarioPorRefreshTokenAsync(refreshToken);

        //    if (usuario == null)
        //    {
        //        throw new SecurityTokenException("Usuario no encontrado para el refresh token");
        //    }

        //    // Generar un nuevo accessToken y opcionalmente un nuevo refreshToken
        //    var (nuevoAccessToken, nuevoRefreshToken) = await _provedorJwt.Crear(usuario);

        //    // Actualizar las cookies con los nuevos tokens
        //    context.Response.Cookies.Append("access_token", nuevoAccessToken, new CookieOptions
        //    {
        //        HttpOnly = true,
        //        Secure = true,
        //        SameSite = SameSiteMode.None,
        //        Expires = DateTime.UtcNow.AddMinutes(60)
        //    });

        //    context.Response.Cookies.Append("refresh_token", nuevoRefreshToken, new CookieOptions
        //    {
        //        HttpOnly = true,
        //        Secure = true,
        //        SameSite = SameSiteMode.None,
        //        Expires = DateTime.UtcNow.AddDays(7) // Ejemplo: refresh tokens duran más tiempo
        //    });

        //    return nuevoAccessToken;
        //}

    }
}
