using System.Collections.Generic;

namespace Servicio.DTOS.Common
{
    /// <summary>
    /// Resultado genérico para consultas paginadas.
    /// </summary>
    public class PaginacionResultado<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => PageSize > 0 ? (int)System.Math.Ceiling((double)TotalCount / PageSize) : 0;
    }
}
