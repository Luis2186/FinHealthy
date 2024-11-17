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
        public Task<T> ObtenerPorIdAsync(int id);
        // Obtener todos los elementos con soporte de paginación y cancelación
        public Task<IEnumerable<T>> ObtenerTodosAsync();
        public Task<bool> CrearAsync(T model);
        public Task<bool> ActualizarAsync(T model);
        public Task<bool> EliminarAsync(string id);
    }
}
