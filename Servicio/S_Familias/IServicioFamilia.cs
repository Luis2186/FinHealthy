using Dominio;
using Dominio.Familias;
using Servicio.DTOS.FamiliasDTO;
using Servicio.DTOS.SolicitudesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.S_Familias
{
    public interface IServicioFamilia
    {
        public Task<Resultado<FamiliaDTO>> ObtenerFamiliaPorId(int id);
        public Task<Resultado<IEnumerable<FamiliaDTO>>> ObtenerTodasLasFamilias();
        public Task<Resultado<FamiliaDTO>> CrearFamilia(CrearFamiliaDTO familiaCreacionDTO);
        public Task<Resultado<FamiliaDTO>> ActualizarFamilia(int familiaId,ActualizarFamiliaDTO familiaActualizacionDTO);
        public Task<Resultado<bool>> EliminarFamilia(int id);
        public Task<Resultado<bool>> IngresarAFamilia(UnirseAFamiliaDTO unionFamiliaDTO);
        public Task<Resultado<bool>> AceptarSolicitudIngresoAFamilia(int idSolicitud);
        public Task<Resultado<bool>> RechazarSolicitudIngresoAFamilia(int idSolicitud);
        public Task<Resultado<SolicitudDTO>> EnviarSolicitudIngresoAFamilia(EnviarSolicitudDTO solicitud);
    }
}
