using Dominio;
using Dominio.Errores;
using Dominio.Gastos;
using Repositorio.Repositorios.Validacion;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.R_Gastos.R_Monedas
{
    public class RepositorioMoneda : RepositorioCRUD<Moneda>, IRepositorioMoneda
    {
        public RepositorioMoneda(ApplicationDbContext context, IValidacion<Moneda> validacion)
            : base(context, validacion)
        {
        }

        // Método específico para buscar por código
        public async Task<Resultado<Moneda>> ObtenerPorCodigoAsync(string codigo, CancellationToken cancellationToken)
        {
            var entidad = await _dbContext.Set<Moneda>().FirstOrDefaultAsync(mon => mon.Codigo == codigo, cancellationToken);
            return entidad == null
                ? Resultado<Moneda>.Failure(ErroresCrud.ErrorDeCreacion(typeof(Moneda).Name))
                : Resultado<Moneda>.Success(entidad);
        }
    }
}
