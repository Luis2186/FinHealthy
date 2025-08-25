using Dominio;
using Dominio.Gastos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Repositorio.Repositorios.Validacion;

namespace Repositorio.Repositorios.R_Gastos
{
    public class RepositorioGasto : RepositorioCRUD<Gasto>, IRepositorioGasto
    {
        private readonly ApplicationDbContext _context;

        public RepositorioGasto(ApplicationDbContext context, IValidacion<Gasto> validacion)
            : base(context, validacion)
        {
            _context = context;
        }

        public async Task<Resultado<List<Gasto>>> ObtenerGastosPorGrupoYUsuarioIncluyendoTodo(int grupoId, string usuarioId, CancellationToken cancellationToken)
        {
            try
            {
                var gastos = await _context.Gastos
                    .Where(g => g.GrupoId == grupoId && g.UsuarioCreadorId == usuarioId)
                    .Include(g => g.Cuotas)
                    .Include(g => g.CompartidoCon)
                    .Include(g => g.SubCategoria)
                    .Include(g => g.MetodoDePago)
                    .Include(g => g.Moneda)
                    .Include(g => g.Grupo)
                    .Include(g => g.UsuarioCreador)
                    .ToListAsync(cancellationToken);
                return Resultado<List<Gasto>>.Success(gastos);
            }
            catch (Exception ex)
            {
                return Resultado<List<Gasto>>.Failure(new Dominio.Abstracciones.Error("RepositorioGasto.ObtenerGastosPorGrupoYUsuarioIncluyendoTodo", ex.Message));
            }
        }
    }
}
