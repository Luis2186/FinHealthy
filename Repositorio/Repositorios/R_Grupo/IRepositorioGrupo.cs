using Dominio;
using Dominio.Grupos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.R_Grupo
{
    public interface IRepositorioGrupo : IRepositorioCRUD<Grupo>
    {
        public Task<Resultado<Grupo>> ObtenerPorIdAsync(int id);
        public Task<Resultado<IEnumerable<Grupo>>> ObtenerTodosAsync();
        public Task<Resultado<Grupo>> ObtenerGrupoPorIdAdministrador(string usuarioAdminId);
        public Task<Resultado<List<Grupo>>> ObtenerGruposPorUsuario(string usuarioId);
        public Task<Resultado<bool>> MiembroExisteEnElGrupo(int idGrupo, string usuarioId);
    }
}
