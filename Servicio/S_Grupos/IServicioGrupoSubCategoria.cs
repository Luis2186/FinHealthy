using Dominio;
using Servicio.DTOS.SubCategoriasDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.S_Grupos
{
    public interface IServicioGrupoSubCategoria
    {
        Task<Resultado<List<GrupoSubCategoriaDTO>>> ObtenerPorGrupoIdAsync(int grupoId, CancellationToken cancellationToken);
        Task<Resultado<GrupoSubCategoriaDTO>> CrearAsync(int grupoId, CrearGrupoSubCategoriaDTO dto, CancellationToken cancellationToken);
        Task<Resultado<GrupoSubCategoriaDTO>> ActualizarAsync(int grupoId, int id, ActualizarGrupoSubCategoriaDTO dto, CancellationToken cancellationToken);
        Task<Resultado<bool>> EliminarAsync(int grupoId, int id, CancellationToken cancellationToken);
        Task<Resultado<bool>> AsignarSubcategoriasBaseAlGrupoAsync(int grupoId, CancellationToken cancellationToken);
    }
}
