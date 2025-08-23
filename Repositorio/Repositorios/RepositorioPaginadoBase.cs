using Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Repositorio.Repositorios
{
    public abstract class RepositorioPaginadoBase<T> where T : class
    {
        protected readonly DbContext _dbContext;
        protected RepositorioPaginadoBase(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Resultado<(IEnumerable<T> Items, int TotalItems)>> ObtenerPaginadoAsync(
            Func<IQueryable<T>, IQueryable<T>> filtro,
            int page, int pageSize, string sortBy, string sortDir, CancellationToken cancellationToken)
        {
            var query = filtro(_dbContext.Set<T>().AsNoTracking());
            var prop = typeof(T).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop != null)
            {
                query = sortDir.ToLower() == "desc"
                    ? query.OrderByDescending(x => EF.Property<object>(x, prop.Name))
                    : query.OrderBy(x => EF.Property<object>(x, prop.Name));
            }
            var totalItems = await query.CountAsync(cancellationToken);
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return Resultado<(IEnumerable<T> Items, int TotalItems)>.Success((items, totalItems));
        }
    }
}
