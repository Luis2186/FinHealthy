using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio
{
    public interface IServiciosCRUD<T> where T : class
    {
        public Task<Resultado<T>> ObtenerPorId (int id);
        public Task<Resultado<IEnumerable<T>>> ObtenerTodas();
        public Task<Resultado<T>> Crear(T model);
        public Task<Resultado<T>> Actualizar(int idModel,T model);
        public Task<Resultado<bool>> Eliminar(int id);
    }
}
