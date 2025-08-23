using AutoMapper;
using Dominio;
using Dominio.Gastos;
using Repositorio.Repositorios.R_Gastos.R_Monedas;
using Repositorio.Repositorios.R_Categoria;
using Repositorio.Repositorios.R_Categoria.R_SubCategoria;
using Repositorio.Repositorios.R_Gastos;
using Repositorio.Repositorios.R_Gastos.R_MetodosDePago;
using Repositorio.Repositorios.Usuarios;
using Servicio.DTOS.GastosDTO;
using Servicio.DTOS.GruposDTO;
using Servicio.S_Categorias;
using Servicio.S_Categorias.S_SubCategorias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Servicio.S_Gastos
{
    public class ServicioGasto : IServicioGasto
    {
        private readonly IRepositorioSubCategoria _repoSubCategoria;
        private readonly IRepositorioMoneda _repoMoneda;
        private readonly IRepositorioMetodoDePago _repoMetodoDePago;
        private readonly IRepositorioUsuario _repoUsuarios;
        private readonly IRepositorioGasto _repoGastos;
        private readonly IMapper _mapper;

        public ServicioGasto(IRepositorioSubCategoria repoSubCategoria, IRepositorioMoneda repoMoneda,
            IRepositorioMetodoDePago repoMetodoDePago, IRepositorioUsuario repoUsuario,
            IRepositorioGasto repoGastos, IMapper mapper)
        {
            _repoSubCategoria = repoSubCategoria;
            _repoMoneda = repoMoneda;
            _repoMetodoDePago = repoMetodoDePago;
            _repoUsuarios = repoUsuario;
            _repoGastos = repoGastos;
            _mapper = mapper;
        }

        public async Task<Resultado<GastoDTO>> CrearGasto(CrearGastoDTO gastoCreacionDTO, CancellationToken cancellationToken)
        {
            var metodoDePagoResult = await _repoMetodoDePago.ObtenerPorIdAsync(gastoCreacionDTO.MetodoDePagoId, cancellationToken);
            if (metodoDePagoResult.TieneErrores) return Resultado<GastoDTO>.Failure(metodoDePagoResult.Errores);

            var categoriaResult = await _repoSubCategoria.ObtenerPorIdAsync(gastoCreacionDTO.SubCategoriaId, cancellationToken);
            if (categoriaResult.TieneErrores) return Resultado<GastoDTO>.Failure(categoriaResult.Errores);

            var monedaResult = await _repoMoneda.ObtenerPorCodigoAsync(gastoCreacionDTO.MonedaId, cancellationToken);
            if (monedaResult.TieneErrores) return Resultado<GastoDTO>.Failure(monedaResult.Errores);

            MetodoDePago metodoDePagoElegido = metodoDePagoResult.Valor;
            SubCategoria subCategoriaElegida = categoriaResult.Valor;
            Moneda monedaElegida = monedaResult.Valor;
  
            Gasto nuevoGasto = new Gasto(subCategoriaElegida, metodoDePagoElegido, monedaElegida, gastoCreacionDTO.FechaDeGasto,
                gastoCreacionDTO.Descripcion, gastoCreacionDTO.Etiqueta,gastoCreacionDTO.Lugar, gastoCreacionDTO.EsFinanciado,
                gastoCreacionDTO.EsCompartido, gastoCreacionDTO.Monto, gastoCreacionDTO.CantidadDeCuotas);

            if (gastoCreacionDTO.EsCompartido)
            {
                var usuariosCompartidosResult = await _repoUsuarios.BuscarUsuarios(gastoCreacionDTO.UsuariosCompartidosIds, cancellationToken);
                if (usuariosCompartidosResult.TieneErrores) return Resultado<GastoDTO>.Failure(usuariosCompartidosResult.Errores);
                
                var resultadoIngreso = nuevoGasto.IngresarGastoCompartido(usuariosCompartidosResult.Valor.ToList());
                if(resultadoIngreso.TieneErrores) return Resultado<GastoDTO>.Failure(resultadoIngreso.Errores);

            } else
            {
                var resultadoIngreso = nuevoGasto.IngresarGastoPersonal();
                if (resultadoIngreso.TieneErrores) return Resultado<GastoDTO>.Failure(resultadoIngreso.Errores);
            }

            var resultado = await _repoGastos.CrearAsync(nuevoGasto, cancellationToken);

            if (resultado.TieneErrores) return Resultado<GastoDTO>.Failure(resultado.Errores);

            var gastoDto = _mapper.Map<GastoDTO>(resultado.Valor);

            return gastoDto;
        }
    }
}
