using Dominio;
using Dominio.Gastos;
using Repositorio.Repositorios.Validacion;

namespace Repositorio.Repositorios.R_Gastos
{
    public class RepositorioGasto : RepositorioCRUD<Gasto>, IRepositorioGasto
    {
        public RepositorioGasto(ApplicationDbContext context, IValidacion<Gasto> validacion)
            : base(context, validacion)
        {
        }
        // Métodos específicos de Gasto pueden ir aquí si son necesarios
    }
}
