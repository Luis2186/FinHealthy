using Dominio;
using Dominio.Familias;
using Servicio.DTOS.FamiliasDTO;
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
        public Task<Resultado<FamiliaDTO>> CrearFamilia(FamiliaCreacionDTO familiaCreacionDTO);
        public Task<Resultado<FamiliaDTO>> ActualizarFamilia(int familiaId,FamiliaActualizacionDTO familiaActualizacionDTO);
        public Task<Resultado<bool>> EliminarFamilia(int id);
    }
}
