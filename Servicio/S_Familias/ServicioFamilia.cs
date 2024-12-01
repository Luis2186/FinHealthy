using AutoMapper;
using Dominio;
using Dominio.Errores;
using Dominio.Familias;
using Repositorio.Repositorios.R_Familias;
using Repositorio.Repositorios.Usuarios;
using Servicio.DTOS.FamiliasDTO;

namespace Servicio.S_Familias
{
    public class ServicioFamilia : IServicioFamilia
    {
        private readonly IRepositorioUsuario _repoUsuarios;
        private readonly IRepositorioFamilia _repoFamilia;
        private readonly IRepositorioMiembroFamilia _repoMiembroFamilia;
        private readonly IMapper _mapper;
        public ServicioFamilia(IRepositorioFamilia repoGrupoFamilia,
                               IRepositorioMiembroFamilia repoMiembroFamilia,
                               IRepositorioUsuario repositorioUsuario,
                               IMapper mapper)
        {
            _repoFamilia = repoGrupoFamilia;
            _repoMiembroFamilia = repoMiembroFamilia;
            _repoUsuarios = repositorioUsuario;
            _mapper = mapper;
        }

        public async Task<Resultado<FamiliaDTO>> ActualizarFamilia(int familiaId,FamiliaActualizacionDTO familiaActDTO)
        {
            try
            {
                var familiaBuscada = await _repoFamilia.ObtenerPorIdAsync(familiaId);

                if (familiaBuscada.TieneErrores) return Resultado<FamiliaDTO>.Failure(familiaBuscada.Errores);

                // Mapeo de los datos del DTO al usuario existente
                _mapper.Map(familiaActDTO, familiaBuscada.Valor); // Actualizar el usuario con el DTO

                var resultado = await _repoFamilia.ActualizarAsync(familiaBuscada.Valor);

                if (resultado.TieneErrores) return Resultado<FamiliaDTO>.Failure(resultado.Errores);

                var familiaDTO = _mapper.Map<FamiliaDTO>(resultado.Valor);

                return Resultado<FamiliaDTO>.Success(familiaDTO);
            }
            catch (Exception ex)
            {
                return Resultado<FamiliaDTO>.Failure(ErroresCrud.ErrorDeExcepcion("Servicio.ActualizarFamilia", ex.Message));
            }
        }

        public async Task<Resultado<FamiliaDTO>> CrearFamilia(FamiliaCreacionDTO familiaCreacionDTO)
        {
            try
            {
                var usuarioAdmin =await _repoUsuarios.ObtenerPorIdAsync(familiaCreacionDTO.UsuarioAdministradorId);

                if (usuarioAdmin.TieneErrores) return Resultado<FamiliaDTO>.Failure(usuarioAdmin.Errores);

                var familia = _mapper.Map<Familia>(familiaCreacionDTO);

                familia.UsuarioAdministrador = usuarioAdmin.Valor;
                familia.EstablecerCodigo(familiaCreacionDTO.CodigoAcceso);

                var resultado = await _repoFamilia.CrearAsync(familia);

                if (resultado.TieneErrores) return Resultado<FamiliaDTO>.Failure(resultado.Errores);

                var familiaDTO = _mapper.Map<FamiliaDTO>(resultado.Valor);

                return Resultado<FamiliaDTO>.Success(familiaDTO);
            }
            catch (Exception ex)
            {
                return Resultado<FamiliaDTO>.Failure(ErroresCrud.ErrorDeExcepcion("Servicio.CrearFamilia", ex.Message));
            }
        }

        public async Task<Resultado<bool>> EliminarFamilia(int id)
        {
            try
            {
                var resultado = await _repoFamilia.EliminarAsync(id);

                if (resultado.TieneErrores) return Resultado<bool>.Failure(resultado.Errores);

                return Resultado<bool>.Success(resultado.Valor);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion("Servicio.EliminarFamilia", ex.Message));
            }
        }

        public async Task<Resultado<FamiliaDTO>> ObtenerFamiliaPorId(int id)
        {
            try
            {
                var resultado = await _repoFamilia.ObtenerPorIdAsync(id);

                if (resultado.TieneErrores) return Resultado<FamiliaDTO>.Failure(resultado.Errores);

                var familiaDTO = _mapper.Map<FamiliaDTO>(resultado.Valor);

                return Resultado<FamiliaDTO>.Success(familiaDTO);
            }
            catch (Exception ex)
            {
                return Resultado<FamiliaDTO>.Failure(ErroresCrud.ErrorDeExcepcion("Servicio.ObtenerFamiliaPorId", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<FamiliaDTO>>> ObtenerTodasLasFamilias()
        {
            try
            {
                var resultado = await _repoFamilia.ObtenerTodosAsync();

                if (resultado.TieneErrores) return Resultado<IEnumerable<FamiliaDTO>>.Failure(resultado.Errores);

                var familiasDTO = _mapper.Map<IEnumerable<FamiliaDTO>>(resultado.Valor);

                return Resultado<IEnumerable<FamiliaDTO>>.Success(familiasDTO);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<FamiliaDTO>>.Failure(ErroresCrud.ErrorDeExcepcion("Servicio.ObtenerTodasLasFamilias", ex.Message));
            }
        }
    }
}
