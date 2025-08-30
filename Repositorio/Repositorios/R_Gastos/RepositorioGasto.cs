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

        public async Task<Resultado<List<Gasto>>> ObtenerGastosPorGrupoYUsuario(int grupoId, string usuarioId,
                                                     CancellationToken cancellationToken, bool esFijo = false)
        {
            try
            {
                var gastos = await _context.Gastos
                    .Where(g => g.GrupoId == grupoId && g.UsuarioCreadorId == usuarioId &&
                        ((esFijo && g is GastoFijo) || (!esFijo && g is GastoMensual)))
                    .Include(g => g.SubCategoria)
                    .Include(g => g.MetodoDePago)
                    .Include(g => g.Moneda)
                    .Include(g => g.Grupo)
                    .Include(g => g.UsuarioCreador)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
                return Resultado<List<Gasto>>.Success(gastos);
            }
            catch (Exception ex)
            {
                return Resultado<List<Gasto>>.Failure(new Dominio.Abstracciones.Error("RepositorioGasto.ObtenerGastosFijosPorGrupoYUsuarioIncluyendoTodo", ex.Message));
            }
        }

        public async Task<Resultado<List<Gasto>>> ObtenerGastosCompartidosPorGrupoYUsuario(int grupoId, string usuarioId,
            CancellationToken cancellationToken, bool esFijo = false)
        {
            try
            {
                var gastos = await _context.Gastos
                    .Where(g => g.GrupoId == grupoId && g.UsuarioCreadorId == usuarioId && g is GastoCompartidoPrincipal)
                    .ToListAsync(cancellationToken);
                var gastosCompartidos = gastos.OfType<GastoCompartidoPrincipal>().ToList();
                foreach (var gasto in gastosCompartidos)
                {
                    _context.Entry(gasto).Collection(x => x.CompartidoCon).Load();
                    foreach (var detalle in gasto.CompartidoCon)
                    {
                        _context.Entry(detalle).Reference(x => x.Miembro).Load();
                    }
                    _context.Entry(gasto).Reference(x => x.SubCategoria).Load();
                    _context.Entry(gasto).Reference(x => x.MetodoDePago).Load();
                    _context.Entry(gasto).Reference(x => x.Moneda).Load();
                    _context.Entry(gasto).Reference(x => x.Grupo).Load();
                    _context.Entry(gasto).Reference(x => x.UsuarioCreador).Load();
                }
                return Resultado<List<Gasto>>.Success(gastosCompartidos.Cast<Gasto>().ToList());
            }
            catch (Exception ex)
            {
                return Resultado<List<Gasto>>.Failure(new Dominio.Abstracciones.Error("RepositorioGasto.ObtenerGastosCompartidosPorGrupoYUsuarioIncluyendoTodo", ex.Message));
            }
        }

        public async Task<Resultado<List<Gasto>>> ObtenerGastosEnCuotasPorGrupoYUsuario(int grupoId, string usuarioId, CancellationToken cancellationToken)
        {
            try
            {
                var gastos = await _context.Gastos
                    .Where(g => g.GrupoId == grupoId && g.UsuarioCreadorId == usuarioId && g is GastoEnCuotas)
                    .ToListAsync(cancellationToken);
                var gastosEnCuotas = gastos.OfType<GastoEnCuotas>().ToList();
                foreach (var gasto in gastosEnCuotas)
                {
                    _context.Entry(gasto).Collection(x => x.Cuotas).Load();
                    _context.Entry(gasto).Reference(x => x.SubCategoria).Load();
                    _context.Entry(gasto).Reference(x => x.MetodoDePago).Load();
                    _context.Entry(gasto).Reference(x => x.Moneda).Load();
                    _context.Entry(gasto).Reference(x => x.Grupo).Load();
                    _context.Entry(gasto).Reference(x => x.UsuarioCreador).Load();
                }
                return Resultado<List<Gasto>>.Success(gastosEnCuotas.Cast<Gasto>().ToList());
            }
            catch (Exception ex)
            {
                return Resultado<List<Gasto>>.Failure(new Dominio.Abstracciones.Error("RepositorioGasto.ObtenerGastosEnCuotasPorGrupoYUsuarioIncluyendoTodo", ex.Message));
            }
        }

        // Método general si se requiere atraer todos los gastos
        public async Task<Resultado<List<Gasto>>> ObtenerGastosPorGrupoYUsuarioIncluyendoTodo(int grupoId, string usuarioId, CancellationToken cancellationToken)
        {
            try
            {
                var gastos = await _context.Gastos
                    .Where(g => g.GrupoId == grupoId && g.UsuarioCreadorId == usuarioId)
                    .ToListAsync(cancellationToken);
                foreach (var gasto in gastos)
                {
                    if (gasto is GastoCompartidoPrincipal compartido)
                    {
                        _context.Entry(compartido).Collection(x => x.CompartidoCon).Load();
                        foreach (var detalle in compartido.CompartidoCon)
                        {
                            _context.Entry(detalle).Reference(x => x.Miembro).Load();
                        }
                    }
                    if (gasto is GastoEnCuotas enCuotas)
                    {
                        _context.Entry(enCuotas).Collection(x => x.Cuotas).Load();
                    }
                    _context.Entry(gasto).Reference(x => x.SubCategoria).Load();
                    _context.Entry(gasto).Reference(x => x.MetodoDePago).Load();
                    _context.Entry(gasto).Reference(x => x.Moneda).Load();
                    _context.Entry(gasto).Reference(x => x.Grupo).Load();
                    _context.Entry(gasto).Reference(x => x.UsuarioCreador).Load();
                }
                return Resultado<List<Gasto>>.Success(gastos);
            }
            catch (Exception ex)
            {
                return Resultado<List<Gasto>>.Failure(new Dominio.Abstracciones.Error("RepositorioGasto.ObtenerGastosPorGrupoYUsuarioIncluyendoTodo", ex.Message));
            }
        }

        public async Task<Resultado<List<Gasto>>> ObtenerGastosPorGrupoYUsuarioPorTipo(int grupoId, string usuarioId, TipoGasto tipoGasto, int? anio, int? mes, CancellationToken cancellationToken)
        {
            try
            {
                var query = _context.Gastos.AsQueryable();
                query = query.Where(g => g.GrupoId == grupoId && g.UsuarioCreadorId == usuarioId);
                if (anio.HasValue)
                    query = query.Where(g => g.FechaDeGasto.Year == anio.Value);
                if (mes.HasValue)
                    query = query.Where(g => g.FechaDeGasto.Month == mes.Value);
                switch (tipoGasto)
                {
                    case TipoGasto.Fijo:
                        query = query.Where(g => g is GastoFijo);
                        break;
                    case TipoGasto.Mensual:
                        query = query.Where(g => g is GastoMensual);
                        break;
                    case TipoGasto.Compartido:
                        query = query.Where(g => g is GastoCompartidoPrincipal);
                        break;
                    case TipoGasto.Cuota:
                        query = query.Where(g => g is GastoEnCuotas);
                        break;
                    case TipoGasto.Todos:
                    default:
                        // No filtrar por tipo
                        break;
                }
                var gastos = await query.ToListAsync(cancellationToken);
                foreach (var gasto in gastos)
                {
                    if (gasto is GastoCompartidoPrincipal compartido)
                    {
                        _context.Entry(compartido).Collection(x => x.CompartidoCon).Load();
                        foreach (var detalle in compartido.CompartidoCon)
                        {
                            _context.Entry(detalle).Reference(x => x.Miembro).Load();
                        }
                    }
                    if (gasto is GastoEnCuotas enCuotas)
                    {
                        _context.Entry(enCuotas).Collection(x => x.Cuotas).Load();
                    }
                    _context.Entry(gasto).Reference(x => x.SubCategoria).Load();
                    _context.Entry(gasto).Reference(x => x.MetodoDePago).Load();
                    _context.Entry(gasto).Reference(x => x.Moneda).Load();
                    _context.Entry(gasto).Reference(x => x.Grupo).Load();
                    _context.Entry(gasto).Reference(x => x.UsuarioCreador).Load();
                }
                return Resultado<List<Gasto>>.Success(gastos);
            }
            catch (Exception ex)
            {
                return Resultado<List<Gasto>>.Failure(new Dominio.Abstracciones.Error("RepositorioGasto.ObtenerGastosPorGrupoYUsuarioPorTipo", ex.Message));
            }
        }
    }
}
