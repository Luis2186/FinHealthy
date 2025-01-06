using Dominio;
using Dominio.Gastos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.Monedas
{
    public interface IRepositorioMoneda : IRepositorioCRUD<Moneda>
    {
        public Task<Resultado<IEnumerable<Moneda>>> ObtenerTodosAsync();
        public Task<Resultado<Moneda>> ObtenerPorIdAsync(string codigo);
    }
}
