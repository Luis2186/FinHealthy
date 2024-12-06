using Dominio.Gastos;
using Repositorio.Repositorios.Validacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.R_Gastos.R_Monedas
{
    public class RepositorioMoneda : RepositorioCRUD<Moneda>, IRepositorioCRUD<Moneda>
    {
        public RepositorioMoneda(ApplicationDbContext context, IValidacion<Moneda> validacion)
            :base(context, validacion)
        {
            
        }
    }
}
