using Dominio;
using Dominio.Errores;
using Dominio.Gastos;
using Dominio.Solicitudes;
using Microsoft.EntityFrameworkCore;
using Repositorio.Repositorios.Validacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.R_Categoria
{
    public class RepositorioCategoria : RepositorioCRUD<Categoria>, IRepositorioCategoria
    {
        private readonly ApplicationDbContext _dbContext;
        public RepositorioCategoria(ApplicationDbContext context, IValidacion<Categoria> validacion)
            : base(context, validacion)
        {
            _dbContext = context;
        }

        public async Task<Resultado<Categoria>> ObtenerPorIdAsync(int id)
        {
            try
            {
                var entidad = await _dbContext.Categorias
                    .Include(cat=> cat.SubCategorias)
                        .ThenInclude(sub => sub.Familia)
                    .FirstOrDefaultAsync(cat => cat.Id == id );

                return entidad == null
                    ? Resultado<Categoria>.Failure(ErroresCrud.ErrorDeCreacion(typeof(Categoria).Name))
                    : Resultado<Categoria>.Success(entidad);
            }
            catch (Exception ex)
            {
                return Resultado<Categoria>.Failure(ErroresCrud.ErrorDeExcepcion($"{typeof(Categoria).Name}.ObtenerPorIdAsync", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<Categoria>>> ObtenerTodosAsync()
        {
            try
            {
                var categorias = _dbContext.Categorias
                    .Include(cat => cat.SubCategorias)
                        .ThenInclude(sub =>sub.Familia)
                    .ToList();

                return Resultado<IEnumerable<Categoria>>.Success(categorias);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<Categoria>>.Failure(ErroresCrud.ErrorDeExcepcion("FIND_ALL", ex.Message));
            }
        }
    }
}
