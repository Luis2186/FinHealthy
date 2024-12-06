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
        public Task<Resultado<T>> ObtenerPorIdAsync(int id);
        public Task<Resultado<IEnumerable<T>>> ObtenerTodosAsync();
        public Task<Resultado<T>> CrearAsync(T model);
        public Task<Resultado<T>> ActualizarAsync(T model);
        public Task<Resultado<bool>> EliminarAsync(int id);
    }
}
