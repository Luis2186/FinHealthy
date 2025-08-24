using Dominio;
using Dominio.Grupos;
using Repositorio.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.R_Grupo
{
    public interface IRepositorioGrupo : IRepositorioCRUD<Grupo>
    {
        Task<Resultado<bool>> MiembroExisteEnElGrupo(int idGrupo, string usuarioId);
        Task<Resultado<List<Grupo>>> ObtenerGruposPorUsuario(string usuarioId);
        Task<Resultado<Grupo>> ObtenerGrupoPorIdAdministrador(string usuarioAdminId);
        Task<Resultado<Grupo>> ObtenerGrupoPorIdAdministrador(string usuarioAdminId, CancellationToken cancellationToken);
        Task<Resultado<Grupo>> ObtenerGrupoPorIdConUsuariosYSubcategorias(int grupoId, CancellationToken cancellationToken);
    }
}
