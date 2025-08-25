using System.Security.Claims;

namespace FinHealthAPI.Extensiones
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public static string? GetUserEmail(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.Email)?.Value;

        public static string? GetUserName(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.Name)?.Value;

        public static string? GetUserRole(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.Role)?.Value;

        public static IEnumerable<string> GetUserRoles(this ClaimsPrincipal user)
            => user.FindAll(ClaimTypes.Role).Select(c => c.Value);

        public static string? GetGivenName(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.GivenName)?.Value;

        public static string? GetSurname(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.Surname)?.Value;

        public static string? GetPhoneNumber(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.MobilePhone)?.Value;

        public static string? GetCountry(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.Country)?.Value;

        public static string? GetBirthDate(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.DateOfBirth)?.Value;
    }
}
