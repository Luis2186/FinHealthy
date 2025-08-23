using System.Collections.Generic;

namespace Servicio.DTOS.Common
{
    /// <summary>
    /// DTO genérico para parámetros de paginación y ordenamiento.
    /// </summary>
    public class PaginacionDTO<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; } = "asc";
        public int TotalPages { get; set; }
    }
}
