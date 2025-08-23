using Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Servicio
{
    public interface IServiciosCRUD<T>
    {
        Task<Resultado<T>> ObtenerPorId(int id, CancellationToken cancellationToken);
        Task<Resultado<IEnumerable<T>>> ObtenerTodas(CancellationToken cancellationToken);
        Task<Resultado<T>> Crear(T model, CancellationToken cancellationToken);
        Task<Resultado<T>> Actualizar(int idModel, T model, CancellationToken cancellationToken);
        Task<Resultado<bool>> Eliminar(int id, CancellationToken cancellationToken);
    }
}
