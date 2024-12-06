using Dominio;
using Dominio.Gastos;
using Repositorio.Repositorios.R_Gastos.R_Categoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.S_Gastos.S_Categoria
{
    public class ServicioCategoria : IServicioCategoria
    {
        private readonly IRepositorioCategoria _repoCategoria;
        public ServicioCategoria(IRepositorioCategoria repoCategoria)
        {
            _repoCategoria = repoCategoria;
        }

        public async Task<Resultado<Categoria>> ActualizarAsync(Categoria model)
        {
            var resultado = await _repoCategoria.ActualizarAsync(model);

            return resultado.Valor;
        }

        public async Task<Resultado<Categoria>> CrearAsync(Categoria model)
        {
            var resultado = await _repoCategoria.CrearAsync(model);

            return resultado.Valor;
        }

        public async Task<Resultado<bool>> EliminarAsync(int id)
        {
            var resultado = await _repoCategoria.EliminarAsync(id);

            return resultado.Valor;
        }

        public async Task<Resultado<Categoria>> ObtenerPorIdAsync(int id)
        {
            var resultado = await _repoCategoria.ObtenerPorIdAsync(id);

            return resultado.Valor;
        }

        public async Task<Resultado<IEnumerable<Categoria>>> ObtenerTodosAsync()
        {
            var resultado = await _repoCategoria.ObtenerTodosAsync();

            return resultado.Valor.ToList();
        }
    }
}
