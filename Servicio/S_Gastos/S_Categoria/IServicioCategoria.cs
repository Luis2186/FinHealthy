using Dominio;
using Dominio.Gastos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.S_Gastos.S_Categoria
{
    public interface IServicioCategoria 
    {
        public Task<Resultado<Categoria>> ObtenerPorIdAsync(int id);
        public Task<Resultado<IEnumerable<Categoria>>> ObtenerTodosAsync();
        public Task<Resultado<Categoria>> CrearAsync(Categoria model);
        public Task<Resultado<Categoria>> ActualizarAsync(Categoria model);
        public Task<Resultado<bool>> EliminarAsync(int id);
    }
}
