using Dominio;
using System.Threading;
using System.Threading.Tasks;

namespace Servicio.ServiciosExternos
{
    public interface IServicioMonedas
    {
        Task<Resultado<bool>> ActualizarMonedasDesdeServicio(CancellationToken cancellationToken);
    }
}
