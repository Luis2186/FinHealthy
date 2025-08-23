using AutoMapper;
using Dominio.Documentos;
using Repositorio.Repositorios.R_Gastos.R_TipoDeDocumento;
using Servicio.DTOS.TipoDeDocumentoDTO;
using Servicio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Servicio.S_TipoDeDocumento
{
    public class ServicioTipoDeDocumento : ServicioCrud<CrearTipoDeDocumentoDTO, ActualizarTipoDeDocumentoDTO, TipoDeDocumentoDTO, TipoDeDocumento>, IServicioTipoDeDocumento
    {
        public ServicioTipoDeDocumento(IRepositorioTipoDeDocumento repoTipoDeDocumento, IMapper mapper)
            : base(repoTipoDeDocumento, mapper)
        {
        }
        // Si necesitas lógica especial, haz override aquí
    }
}
