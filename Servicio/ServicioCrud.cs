using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dominio;
using Repositorio.Repositorios;

namespace Servicio
{
    public class ServicioCrud<TDtoInsert, TDtoUpdate, TDto, TEntity> : IServicioCrud<TDto, TDtoInsert, TDtoUpdate, TEntity>
        where TEntity : class
    {
        protected readonly IRepositorioCRUD<TEntity> _repo;
        protected readonly IMapper _mapper;

        public ServicioCrud(IRepositorioCRUD<TEntity> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public virtual async Task<Resultado<TDto>> CrearAsync(TDtoInsert dto, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<TEntity>(dto);
            var resultado = await _repo.CrearAsync(entity, cancellationToken);
            if (resultado.TieneErrores) return Resultado<TDto>.Failure(resultado.Errores);
            return _mapper.Map<TDto>(resultado.Valor);
        }

        public virtual async Task<Resultado<TDto>> ActualizarAsync(int id, TDtoUpdate dto, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<TEntity>(dto);
            var resultado = await _repo.ActualizarAsync(entity, cancellationToken);
            if (resultado.TieneErrores) return Resultado<TDto>.Failure(resultado.Errores);
            return _mapper.Map<TDto>(resultado.Valor);
        }

        public virtual async Task<Resultado<bool>> EliminarAsync(int id, CancellationToken cancellationToken)
        {
            return await _repo.EliminarAsync(id, cancellationToken);
        }

        public virtual async Task<Resultado<TDto>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken)
        {
            var resultado = await _repo.ObtenerPorIdAsync(id, cancellationToken);
            if (resultado.TieneErrores) return Resultado<TDto>.Failure(resultado.Errores);
            return _mapper.Map<TDto>(resultado.Valor);
        }

        public virtual async Task<Resultado<IEnumerable<TDto>>> ObtenerTodosAsync(CancellationToken cancellationToken)
        {
            var resultado = await _repo.ObtenerTodosAsync(cancellationToken);
            if (resultado.TieneErrores) return Resultado<IEnumerable<TDto>>.Failure(resultado.Errores);
            var dtos = _mapper.Map<IEnumerable<TDto>>(resultado.Valor);
            return Resultado<IEnumerable<TDto>>.Success(dtos);
        }

        public virtual async Task<Resultado<(IEnumerable<TDto> Items, int TotalItems)>> ObtenerPaginadoAsync(
            int pagina,
            int tamanoPagina,
            string campoOrden,
            string direccionOrden,
            CancellationToken cancellationToken)
        {
            // Implementación básica delegando al repositorio
            var resultado = await _repo.ObtenerPaginadoAsync(q => q, pagina, tamanoPagina, campoOrden, direccionOrden, cancellationToken);
            if (resultado.TieneErrores) return Resultado<(IEnumerable<TDto> Items, int TotalItems)>.Failure(resultado.Errores);
            var items = _mapper.Map<IEnumerable<TDto>>(resultado.Valor.Items);
            return Resultado<(IEnumerable<TDto> Items, int TotalItems)>.Success((items, resultado.Valor.TotalItems));
        }
    }
}
