using System;
using System.Linq;
using System.Linq.Expressions;

namespace Repositorio.Helpers
{
    public class PaginacionOrdenamientoHelper<T> : IPaginacionOrdenamiento<T>
    {
        public IQueryable<T> Ordenar(IQueryable<T> consulta, string? campoOrden, string? direccionOrden)
        {
            if (string.IsNullOrEmpty(campoOrden)) return consulta;

            var parametro = Expression.Parameter(typeof(T), "x");
            var propiedad = Expression.PropertyOrField(parametro, campoOrden);
            var lambda = Expression.Lambda(propiedad, parametro);

            var metodo = direccionOrden?.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";
            var tipos = new Type[] { typeof(T), propiedad.Type };
            var llamada = Expression.Call(typeof(Queryable), metodo, tipos, consulta.Expression, Expression.Quote(lambda));
            return consulta.Provider.CreateQuery<T>(llamada);
        }

        public IQueryable<T> Paginar(IQueryable<T> consulta, int pagina, int tamanoPagina)
        {
            if (pagina < 1) pagina = 1;
            if (tamanoPagina < 1) tamanoPagina = 10;
            return consulta.Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina);
        }
    }
}
