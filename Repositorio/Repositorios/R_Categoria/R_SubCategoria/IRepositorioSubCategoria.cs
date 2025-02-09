using Dominio;
using Dominio.Gastos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.R_Categoria.R_SubCategoria
{
    public interface IRepositorioSubCategoria : IRepositorioCRUD<SubCategoria>
    {
        public Task<Resultado<IEnumerable<SubCategoria>>> ObtenerTodasPorGrupoYCategoria(int grupoId,int categoriaId);
        public Task<Resultado<SubCategoria>> ObtenerPorIdAsync(int id);
    }
}
