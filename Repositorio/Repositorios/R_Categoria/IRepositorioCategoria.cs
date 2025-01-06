using Dominio;
using Dominio.Gastos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.R_Categoria
{
    public interface IRepositorioCategoria : IRepositorioCRUD<Categoria>
    {
        public Task<Resultado<Categoria>> ObtenerPorIdAsync(int id);
        public Task<Resultado<IEnumerable<Categoria>>> ObtenerTodosAsync();
    }
}
