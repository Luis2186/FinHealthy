using Servicio.DTOS.TipoDeDocumentoDTO;
using Dominio.Documentos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Servicio.S_TipoDeDocumento
{
    public interface IServicioTipoDeDocumento : IServicioCrud<TipoDeDocumentoDTO, CrearTipoDeDocumentoDTO, ActualizarTipoDeDocumentoDTO, TipoDeDocumento>
    {
        // M�todos espec�ficos pueden agregarse aqu� si es necesario
    }
}
