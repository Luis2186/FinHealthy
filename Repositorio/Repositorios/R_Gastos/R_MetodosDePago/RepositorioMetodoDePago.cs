using Dominio.Gastos;
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
    }
}
