using Dominio;
using Dominio.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios
{
    public interface IRepositorioCRUD<T>
    {
        public Task<Resultado<T>> ObtenerPorIdAsync(int id);
        // Obtener todos los elementos con soporte de paginación y cancelación
        public Task<Resultado<IEnumerable<T>>> ObtenerTodosAsync();
        public Task<Resultado<T>> CrearAsync(T model);
        public Task<Resultado<T>> ActualizarAsync(T model);
        public Task<Resultado<bool>> EliminarAsync(string id);
    }
}
