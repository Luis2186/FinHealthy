using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;

namespace FinHealthAPI.ConfigOpcionesJwt
{
    public class ConfigJwtOpciones : IConfigureOptions<ConfigBearerOpciones>
    {
        private const string NombreDeSeccion = "Jwt";
        private readonly IConfiguration _configuracion;

        public ConfigJwtOpciones(IConfiguration configuracion)
        {
            _configuracion = configuracion;
        }
        public void Configure(ConfigBearerOpciones options)
        {
            _configuracion.GetSection(NombreDeSeccion).Bind(options);
        }
    }
}
