using Dominio;
using Dominio.Documentos;
using Dominio.Errores;
using Dominio.Gastos;
using Microsoft.EntityFrameworkCore;
using Repositorio.Repositorios.R_Gastos.R_TipoDeDocumento;
using Repositorio.Repositorios.Validacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.R_Gastos.R_TipoDeCambios
{
    public class RepositorioTipoDeDocumento : RepositorioCRUD<TipoDeDocumento>, IRepositorioTipoDeDocumento
    {
        private readonly ApplicationDbContext _dbContext;
        public RepositorioTipoDeDocumento(ApplicationDbContext context, IValidacion<TipoDeDocumento> validacion)
            : base(context, validacion)
        {
            _dbContext = context;
        }

        public async Task<Resultado<TipoDeDocumento>> ObtenerPorIdAsync(int id)
        {
            try
            {
                var entidad = await _dbContext.TipoDeDocumentos
                    .FirstOrDefaultAsync(doc => doc.Id == id);

                return entidad == null
                    ? Resultado<TipoDeDocumento>.Failure(ErroresCrud.ErrorDeCreacion(typeof(TipoDeDocumento).Name))
                    : Resultado<TipoDeDocumento>.Success(entidad);
            }
            catch (Exception ex)
            {
                return Resultado<TipoDeDocumento>.Failure(ErroresCrud.ErrorDeExcepcion($"{typeof(TipoDeDocumento).Name}.ObtenerPorIdAsync", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<TipoDeDocumento>>> ObtenerTodosAsync()
        {
            try
            {
                var categorias = _dbContext.TipoDeDocumentos
                    .ToList();

                return Resultado<IEnumerable<TipoDeDocumento>>.Success(categorias);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<TipoDeDocumento>>.Failure(ErroresCrud.ErrorDeExcepcion("FIND_ALL", ex.Message));
            }
        }


    }
}
