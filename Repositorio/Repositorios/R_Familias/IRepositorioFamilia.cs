using Dominio;
using Dominio.Familias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.R_Familias
{
    public interface IRepositorioFamilia : IRepositorioCRUD<Familia>
    {
        public Task<Resultado<Familia>> ObtenerPorIdAsync(int id);
        public Task<Resultado<IEnumerable<Familia>>> ObtenerTodosAsync();
        public Task<Resultado<Familia>> ObtenerFamiliaPorIdAdministrador(string usuarioAdminId);
        public Task<Resultado<bool>> MiembroExisteEnLaFamilia(int idFamilia, string usuarioId);
    }
}
