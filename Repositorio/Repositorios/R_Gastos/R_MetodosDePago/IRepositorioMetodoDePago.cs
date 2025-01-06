using Dominio.Documentos;
using Dominio;
using Dominio.Gastos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.R_Gastos.R_MetodosDePago
{
    public interface IRepositorioMetodoDePago : IRepositorioCRUD<MetodoDePago>
    {
        public Task<Resultado<MetodoDePago>> ObtenerPorIdAsync(int id);
        public Task<Resultado<IEnumerable<MetodoDePago>>> ObtenerTodosAsync();
    }
}
