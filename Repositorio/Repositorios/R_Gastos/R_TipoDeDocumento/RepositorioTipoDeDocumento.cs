using Dominio.Documentos;
using Repositorio.Repositorios.Validacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.R_Gastos.R_TipoDeCambios
{
    public class RepositorioTipoDeDocumento :RepositorioCRUD<TipoDeDocumento>, IRepositorioCRUD<TipoDeDocumento>
    {
        private readonly ApplicationDbContext _dbContext;
        public RepositorioTipoDeDocumento(ApplicationDbContext context, IValidacion<TipoDeDocumento> validacion)
            : base(context, validacion)
        {
            _dbContext = context;
        }
    }
}
