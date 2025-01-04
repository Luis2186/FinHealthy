using Dominio;
using Dominio.Gastos;
using Servicio.DTOS.CategoriasDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.S_Categorias
{
    public interface IServicioCategoria
    {
        public Task<Resultado<CategoriaDTO>> ObtenerCategoriaPorId(int id);
        public Task<Resultado<IEnumerable<CategoriaDTO>>> ObtenerTodasLasCategorias();
        public Task<Resultado<CategoriaDTO>> CrearCategoria(CategoriaDTO categoriaCreacionDTO);
        public Task<Resultado<CategoriaDTO>> ActualizarCategoria(int categoriaId, CategoriaDTO categoriaActualizacionDTO);
        public Task<Resultado<bool>> EliminarCategoria(int id);
    }
}