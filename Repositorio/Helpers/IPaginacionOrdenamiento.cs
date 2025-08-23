using System.Linq;

namespace Repositorio.Helpers
{
    public interface IPaginacionOrdenamiento<T>
    {
        IQueryable<T> Ordenar(IQueryable<T> consulta, string? campoOrden, string? direccionOrden);
        IQueryable<T> Paginar(IQueryable<T> consulta, int pagina, int tamanoPagina);
    }
}
