using Dominio;
using Dominio.Gastos;
using Repositorio.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.R_Gastos.R_Monedas
{
    public interface IRepositorioMoneda : IRepositorioCRUD<Moneda>
    {
        Task<Resultado<Moneda>> ObtenerPorCodigoAsync(string codigo, CancellationToken cancellationToken);
    }
}
