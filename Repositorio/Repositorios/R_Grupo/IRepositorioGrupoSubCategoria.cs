using Dominio;
using Dominio.Abstracciones;
using Dominio.Grupos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.R_Grupo
{
    public interface IRepositorioGrupoSubCategoria : IRepositorioCRUD<GrupoSubCategoria>
    {
        Task<Resultado<List<GrupoSubCategoria>>> ObtenerPorGrupoIdAsync(int grupoId, CancellationToken cancellationToken);
        Task<Resultado<GrupoSubCategoria>> ObtenerPorGrupoYSubCategoriaAsync(int grupoId, int subCategoriaId, CancellationToken cancellationToken);
        Task<Resultado<bool>> ExisteEnGrupoAsync(int grupoId, int subCategoriaId, CancellationToken cancellationToken);
    }
}
