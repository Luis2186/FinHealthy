using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dominio;

namespace Servicio
{
    public interface IServicioCrud<TDto, TDtoInsert, TDtoUpdate, TEntity>
    {
        Task<Resultado<TDto>> CrearAsync(TDtoInsert dto, CancellationToken cancellationToken);
        Task<Resultado<TDto>> ActualizarAsync(int id, TDtoUpdate dto, CancellationToken cancellationToken);
        Task<Resultado<bool>> EliminarAsync(int id, CancellationToken cancellationToken);
        Task<Resultado<TDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken);
        Task<Resultado<IEnumerable<TDto>>> ObtenerTodosAsync(CancellationToken cancellationToken);
        Task<Resultado<(IEnumerable<TDto> Items, int TotalItems)>> ObtenerPaginadoAsync(
            int pagina,
            int tamanoPagina,
            string campoOrden,
            string direccionOrden,
            CancellationToken cancellationToken);
    }
}
