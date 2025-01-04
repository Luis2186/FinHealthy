using Dominio;
using Dominio.Errores;
using Dominio.Gastos;
using Dominio.Solicitudes;
using Microsoft.EntityFrameworkCore;
using Repositorio.Repositorios.Validacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.R_Categoria.R_SubCategoria
{
    public class RepositorioSubCategoria : RepositorioCRUD<SubCategoria>, IRepositorioSubCategoria
    {
        private readonly ApplicationDbContext _dbContext;
        public RepositorioSubCategoria(ApplicationDbContext context, IValidacion<SubCategoria> validacion)
            : base(context, validacion)
        {
            _dbContext = context;
        }

        public async Task<Resultado<IEnumerable<SubCategoria>>> ObtenerTodasPorFamiliaYCategoria(int familiaId, int categoriaId)
        {
            try
            {
                var subcategorias = _dbContext.SubCategorias
                    .Include(cat => cat.Familia)
                    .Include(cat => cat.Categoria)
                    .Where(cat => cat.FamiliaId == familiaId && cat.CategoriaId == categoriaId).ToList();
                return Resultado<IEnumerable<SubCategoria>>.Success(subcategorias);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<SubCategoria>>.Failure(ErroresCrud.ErrorDeExcepcion("FIND_ALL_BY_FAMILIACATEGORIA_ID", ex.Message));
            }

        }
    }
}
