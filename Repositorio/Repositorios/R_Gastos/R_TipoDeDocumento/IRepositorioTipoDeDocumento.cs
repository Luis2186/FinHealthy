using Dominio.Gastos;
using Dominio;
using Repositorio.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Documentos;

namespace Repositorio.Repositorios.R_Gastos.R_TipoDeDocumento
{
    public interface IRepositorioTipoDeDocumento : IRepositorioCRUD<TipoDeDocumento>
    {
        public Task<Resultado<TipoDeDocumento>> ObtenerPorIdAsync(int id);
        public Task<Resultado<IEnumerable<TipoDeDocumento>>> ObtenerTodosAsync();
    }
}
