using Dominio.Errores;
using Dominio;
using Dominio.Gastos;
using Microsoft.EntityFrameworkCore;
using Repositorio.Repositorios.Validacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.R_Gastos.R_Monedas
{
    public class RepositorioMoneda : RepositorioCRUD<Moneda>, IRepositorioMoneda
    {
        private readonly ApplicationDbContext _dbContext;
        public RepositorioMoneda(ApplicationDbContext context, IValidacion<Moneda> validacion)
            :base(context, validacion)
        {
            _dbContext = context;
        }

        public async Task<Resultado<Moneda>> ObtenerPorIdAsync(string codigo)
        {
            try
            {
                var entidad = await _dbContext.Monedas
                    .FirstOrDefaultAsync(mon => mon.Codigo == codigo);

                return entidad == null
                    ? Resultado<Moneda>.Failure(ErroresCrud.ErrorDeCreacion(typeof(Moneda).Name))
                    : Resultado<Moneda>.Success(entidad);
            }
            catch (Exception ex)
            {
                return Resultado<Moneda>.Failure(ErroresCrud.ErrorDeExcepcion($"{typeof(Moneda).Name}.ObtenerPorIdAsync", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<Moneda>>> ObtenerTodosAsync()
        {
            try
            {
                var categorias = _dbContext.Monedas
                    .ToList();

                return Resultado<IEnumerable<Moneda>>.Success(categorias);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<Moneda>>.Failure(ErroresCrud.ErrorDeExcepcion("FIND_ALL", ex.Message));
            }
        }


    }
}
