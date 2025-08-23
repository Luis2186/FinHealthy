using Azure;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using Repositorio.Repositorios.Token;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;

namespace FinHealthAPI.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TokenValidationMiddleware> _logger;
        private readonly IConfiguration configuration;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TokenValidationMiddleware(RequestDelegate next,
            ILogger<TokenValidationMiddleware> logger, IConfiguration _config,
            IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _logger = logger;
            configuration = _config;
            _serviceScopeFactory = serviceScopeFactory;
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
            using var scope = _serviceScopeFactory.CreateScope();

            var refreshTokenRepo = scope.ServiceProvider.GetRequiredService<IRepositorioRefreshToken>();
            
            try
            {
                var refreshTokenValido = await ValidateRefreshToken(refreshToken, context.RequestAborted);
                // Intentar validar el accessToken
                if (!string.IsNullOrEmpty(accessToken))
                {
                    principal = ValidateToken(accessToken);
                    var usuarioId = principal.Claims.FirstOrDefault().Value;
                    var resultado = await refreshTokenRepo.RevocarTokensAntiguos(usuarioId,1);
                }

            }
            catch (Exception ex)
            {

                context.Response.Cookies.Append("access_token", "", new CookieOptions
                {
                    HttpOnly = true,   // No accesible desde JavaScript
                    Secure = true,     // Solo se enviará a través de HTTPS
                    SameSite = SameSiteMode.None, // Asegura que no se envíe en solicitudes de terceros
                    Expires = DateTime.Now.AddDays(-1), // Expira en 1 día para eliminarla
                    Path = "/"         // El dominio de la cookie debe coincidir con el path original
                });

                context.Response.Cookies.Append("refresh_token", "", new CookieOptions
                {
                    HttpOnly = true,   // No accesible desde JavaScript
                    Secure = true,     // Solo se enviará a través de HTTPS
                    SameSite = SameSiteMode.None, // Asegura que no se envíe en solicitudes de terceros
                    Expires = DateTime.Now.AddDays(-1), // Expira en 1 día para eliminarla
                    Path = "/"         // El dominio de la cookie debe coincidir con el path original
                });

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
            var accessTokenCookieName = configuration.GetValue<string>("Jwt:AccessTokenCookieName");
            var refreshTokenCookieName = configuration.GetValue<string>("Jwt:refreshTokenCookieName");
            string accessToken = context.Request.Cookies[accessTokenCookieName];
            string refreshToken = context.Request.Cookies[refreshTokenCookieName];

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

        private async Task<bool> ValidateRefreshToken(string refreshToken, CancellationToken cancellationToken)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var refreshTokenRepo = scope.ServiceProvider.GetRequiredService<IRepositorioRefreshToken>();
                var resultado = await refreshTokenRepo.ObtenerPorToken(refreshToken, cancellationToken);
                if (resultado.TieneErrores) throw new UnauthorizedAccessException("Refresh Token inválido o inexistente");
                var token = resultado.Valor;
                var refreshTokenValido = true;
                if (token != null && token.TokenExpirado)
                {
                    refreshTokenValido = false;
                    await refreshTokenRepo.Revocar(token, cancellationToken);
                }
                return refreshTokenValido;
            }
            catch (SecurityTokenException ex)
            {
                throw new UnauthorizedAccessException("Token inválido", ex);
            }
        }
    }
}
