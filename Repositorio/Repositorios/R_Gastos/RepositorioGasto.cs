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

namespace Repositorio.Repositorios.R_Gastos
{
    public class RepositorioGasto : RepositorioCRUD<Gasto>, IRepositorioGasto
    {
        private readonly ApplicationDbContext _dbContext;
        public RepositorioGasto(ApplicationDbContext context, IValidacion<Gasto> validacion)
            :base(context, validacion)
        {
                _dbContext = context;
        }

    }
}
