using Dominio.Gastos;
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


    }
}
