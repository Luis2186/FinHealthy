using Microsoft.IdentityModel.Tokens;
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
            ILogger<TokenValidationMiddleware> logger, IConfiguration _config)
        {
            _next = next;
            _logger = logger;
            configuration = _config;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Intentar obtener el token del header o cookies
            var token = GetTokenFromRequest(context);

            // Si no hay token, simplemente continua con la solicitud
            if (token == null)
            {
                await _next(context);
                return;
            }

            try
            {
                var principal = ValidateToken(token);
                context.Response.Cookies.Append("accessToken", token, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddMinutes(60),
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });
                // Si el token es válido, lo agregamos al contexto
                context.Items["User"] = principal;
            }
            catch (Exception ex)
            {
                // Si la validación falla, respondemos con Unauthorized
                _logger.LogError("Token no válido: " + ex.Message);
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Unauthorized: Invalid token");
                return;
            }

            // Continuamos con el pipeline de la solicitud
            await _next(context);
        }

        // Método para extraer el token del header o cookies
        private string GetTokenFromRequest(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                token = context.Request.Cookies["token"];
            }
            return token;
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
    }
}
