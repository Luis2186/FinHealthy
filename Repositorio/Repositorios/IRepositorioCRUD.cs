using Dominio;
using Dominio.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Repositorio.Repositorios
{
    public interface IRepositorioCRUD<T>
    {
        public Task<Resultado<T>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken);
        public Task<Resultado<IEnumerable<T>>> ObtenerTodosAsync(CancellationToken cancellationToken);
        public Task<Resultado<T>> CrearAsync(T model, CancellationToken cancellationToken);
        public Task<Resultado<T>> ActualizarAsync(T model, CancellationToken cancellationToken);
        public Task<Resultado<bool>> EliminarAsync(int id, CancellationToken cancellationToken);
        /// <summary>
        /// Obtiene una página de resultados ordenados desde la base de datos.
        /// </summary>
        /// <param name="filtro">Función para filtrar el IQueryable.</param>
        /// <param name="pagina">Número de página (1-based).</param>
        /// <param name="tamanoPagina">Cantidad de elementos por página.</param>
        /// <param name="campoOrden">Campo por el que se ordena.</param>
        /// <param name="direccionOrden">Dirección de ordenamiento ("asc" o "desc").</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Tupla con los elementos y el total de elementos.</returns>
        Task<Resultado<(IEnumerable<T> Items, int TotalItems)>> ObtenerPaginadoAsync(
            Func<IQueryable<T>, IQueryable<T>> filtro,
            int pagina,
            int tamanoPagina,
            string campoOrden,
            string direccionOrden,
            CancellationToken cancellationToken);
    }
}
