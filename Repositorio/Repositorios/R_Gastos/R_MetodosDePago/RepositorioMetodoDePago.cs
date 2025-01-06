using Dominio.Documentos;
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

namespace Repositorio.Repositorios.R_Gastos.R_MetodosDePago
{
    public class RepositorioMetodoDePago : RepositorioCRUD<MetodoDePago>, IRepositorioMetodoDePago
    {
        private readonly ApplicationDbContext _dbContext;
        public RepositorioMetodoDePago(ApplicationDbContext context, IValidacion<MetodoDePago> validacion)
            :base(context, validacion)
        {
                _dbContext = context;
        }


        public async Task<Resultado<MetodoDePago>> ObtenerPorIdAsync(int id)
        {
            try
            {
                var entidad = await _dbContext.MetodosDePago
                    .FirstOrDefaultAsync(doc => doc.Id == id);

                return entidad == null
                    ? Resultado<MetodoDePago>.Failure(ErroresCrud.ErrorDeCreacion(typeof(MetodoDePago).Name))
                    : Resultado<MetodoDePago>.Success(entidad);
            }
            catch (Exception ex)
            {
                return Resultado<MetodoDePago>.Failure(ErroresCrud.ErrorDeExcepcion($"{typeof(MetodoDePago).Name}.ObtenerPorIdAsync", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<MetodoDePago>>> ObtenerTodosAsync()
        {
            try
            {
                var categorias = _dbContext.MetodosDePago
                    .ToList();

                return Resultado<IEnumerable<MetodoDePago>>.Success(categorias);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<MetodoDePago>>.Failure(ErroresCrud.ErrorDeExcepcion("FIND_ALL", ex.Message));
            }
        }


    }
}
