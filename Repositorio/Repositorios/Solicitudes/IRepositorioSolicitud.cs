using Dominio;
using Dominio.Solicitudes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.Solicitudes
{
    public interface IRepositorioSolicitud : IRepositorioCRUD<SolicitudUnionFamilia>
    {
        public Task<Resultado<SolicitudUnionFamilia>> ObtenerPorIdAsync(int id);
        public Task<Resultado<IEnumerable<SolicitudUnionFamilia>>> ObtenerTodosAsync();
        public Task<Resultado<bool>> AceptarSolicitud(int idSolicitud);
        public Task<Resultado<bool>> RechazarSolicitud(int idSolicitud);
        public Task<Resultado<IEnumerable<SolicitudUnionFamilia>>> ObtenerTodasPorAdministrador(string idAdministrador,string estado);
    }
}
