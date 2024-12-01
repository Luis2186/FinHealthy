using Dominio;
using Servicio.DTOS.FamiliasDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.S_Familias
{
    public interface IServicioMiembroFamilia
    {
        public Task<Resultado<FamiliaDTO>> ObtenerFamiliaPorId(int id);
        public Task<Resultado<IEnumerable<FamiliaDTO>>> ObtenerTodasLasFamilias();
        public Task<Resultado<FamiliaDTO>> CrearFamilia(CrearFamiliaDTO familiaCreacionDTO);
        public Task<Resultado<FamiliaDTO>> ActualizarFamilia(int familiaId, ActualizarFamiliaDTO familiaActualizacionDTO);
        public Task<Resultado<bool>> EliminarFamilia(int id);
    }
}
