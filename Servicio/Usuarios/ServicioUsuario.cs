using AutoMapper;
using Azure;
using Azure.Core;
using Dominio;
using Dominio.Abstracciones;
using Dominio.Grupos;
using Dominio.Usuarios;
using Microsoft.AspNetCore.Http;
using Repositorio.Repositorios;
using Repositorio.Repositorios.R_Grupo;
using Repositorio.Repositorios.Token;
using Repositorio.Repositorios.Usuarios;
using Servicio.Authentication;
using Servicio.DTOS.UsuariosDTO;
using System.Security.Claims;

namespace Servicio.Usuarios
{
    public class ServicioUsuario : IServicioUsuario
    {
        private readonly IMapper _mapper;
        private readonly IRepositorioUsuario _repoUsuario;
        private readonly IRepositorioGrupo _repoGrupo;
        private readonly IRepositorioRefreshToken _repoRefreshToken;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ProveedorToken _provedorJwt;

        public ServicioUsuario(IRepositorioUsuario repositorioUsuario,
            IMapper mapper, IUnitOfWork unitOfWork, IRepositorioRefreshToken repoRefreshToken,
            IRepositorioGrupo repoGrupo, ProveedorToken provedorJwt)
        {
            _mapper = mapper;
            _repoUsuario = repositorioUsuario;
            _unitOfWork = unitOfWork;
            _provedorJwt = provedorJwt;
            _repoRefreshToken = repoRefreshToken;
            _repoGrupo = repoGrupo;
        }
        public async Task<Resultado<Usuario>> Actualizar(string id, ActualizarUsuarioDTO usuarioDto)
        {
            var usuarioBuscado = await ObtenerPorId(id);

            if (usuarioBuscado.TieneErrores) return Resultado<Usuario>.Failure(usuarioBuscado.Errores);

            // Mapeo de los datos del DTO al usuario existente
            _mapper.Map(usuarioDto, usuarioBuscado.Valor); // Actualizar el usuario con el DTO

            var usuarioActualizado = await _repoUsuario.ActualizarAsync(usuarioBuscado.Valor);

            return usuarioActualizado;
        }

        public async Task<Resultado<bool>> AgregarClaim(string usuarioId, string tipoClaim, string claim)
        {
            return await _repoUsuario.AgregarClaim(usuarioId, tipoClaim, claim);
        }

        public async Task<Resultado<bool>> AgregarRol(string usuarioId, string idRol, string nombreRol)
        {
            return await _repoUsuario.AgregarRol(usuarioId, idRol, nombreRol);
        }

        public async Task<Resultado<(string AccessToken, string RefreshToken, string usuarioId)>>
            Registrar(CrearUsuarioDTO usuarioDto)
        {
            if(usuarioDto == null) return Resultado<(string, string, string)>.Failure(ErroresUsuario.Datos_Invalidos("Registrar"));

            await _unitOfWork.IniciarTransaccionAsync();

            Usuario usuario = _mapper.Map<Usuario>(usuarioDto);

            var usuarioCreado = await _repoUsuario.CrearAsync(usuario, usuarioDto.Password);

            if (usuarioDto.CrearGrupo)
            {
                if(usuarioDto.Grupo == null) return Resultado<(string, string, string)>.Failure(ErroresGrupo.Datos_Invalidos("Registrar"));

                var grupoNuevo = new Grupo(usuario, usuarioDto.Grupo.Nombre ?? "", usuarioDto.Grupo.Descripcion ?? "",
                    usuarioDto.Grupo.CodigoAcceso ?? "");

                usuario.GrupoDeGastos = grupoNuevo;

                await _repoGrupo.CrearAsync(grupoNuevo);
            }
            
            if (usuarioCreado.TieneErrores) return Resultado<(string,string,string)>.Failure(usuarioCreado.Errores);

            if (usuarioDto.Rol != null && usuarioDto.Rol.Trim() != "")
            {
                var rol = await _repoUsuario.BuscarRol("", usuarioDto.Rol.Trim());

                if (rol.TieneErrores) return Resultado<(string,string,string)>.Failure(rol.Errores);
                
                var rolesAgregados = await _repoUsuario.AgregarRol(usuario.Id,rol.Valor.Id, rol.Valor.Name);

                if(rolesAgregados.TieneErrores) return Resultado<(string,string,string)>.Failure(rolesAgregados.Errores);
            }

            var (accessToken, refreshToken) = await _provedorJwt.GenerarTokens(usuario,null!);

            usuario.AsignarToken(accessToken, refreshToken);

            await _unitOfWork.ConfirmarTransaccionAsync();
            return Resultado<(string, string,string)>.Success((accessToken, refreshToken,usuario.Id)); ;
        }

        public async Task<Resultado<bool>> Eliminar(string id)
        {
           return await _repoUsuario.EliminarAsync(id);
        }

        public async Task<Resultado<Usuario>> ObtenerPorId(string id)
        {
            return await _repoUsuario.ObtenerPorIdAsync(id);
        }

        public async Task<Resultado<IEnumerable<string>>> ObtenerRolesPorUsuario(string usuarioId)
        {
            return await _repoUsuario.ObtenerRolesPorUsuario(usuarioId);
        }
    
        public async Task<Resultado<IEnumerable<Usuario>>> ObtenerTodos()
        {
            var usuarios = await _repoUsuario.ObtenerTodosAsync();

            var listaDeUsuarios = usuarios.Valor.ToList();

            foreach (var usuario in listaDeUsuarios)
            {
                usuario.Roles = _repoUsuario.ObtenerRolesPorUsuario(usuario.Id).Result.Valor.ToList();
            }

            return Resultado<IEnumerable<Usuario>>.Success(listaDeUsuarios);
        }

        public async Task<Resultado<IEnumerable<Claim>>> ObtenerTodosLosClaim(string usuarioId)
        {
            return await _repoUsuario.ObtenerTodosLosClaim(usuarioId);
        }

        public async Task<Resultado<IEnumerable<string>>> ObtenerTodosLosRoles()
        {
            return await _repoUsuario.ObtenerTodosLosRoles();
        }

        public async Task<Resultado<bool>> RemoverClaim(string usuarioId, string tipoClaim, string claim)
        {
            return await _repoUsuario.RemoverClaim(usuarioId, tipoClaim, claim);
        }

        public async Task<Resultado<bool>> RemoverRol(string usuarioId, string idRol, string nombreRol)
        {
            return await _repoUsuario.RemoverRol(usuarioId, idRol , nombreRol);
        }

        public async Task<Resultado<(string AccessToken, string RefreshToken,string usuarioId)>> Login(UsuarioLoginDTO usuario)
        {
            var usuarioBuscado = await _repoUsuario.ObtenerPorEmailAsync(usuario.Email);

            if (usuarioBuscado.TieneErrores || usuarioBuscado.Valor == null) return Resultado<(string, string, string)>.Failure(ErroresUsuario.Login("Login"));

            var usuarioLogueadoResultado = await _repoUsuario.Login(usuarioBuscado.Valor, usuario.Password ?? "");

            if(usuarioLogueadoResultado.TieneErrores) return Resultado<(string,string,string)>.Failure(usuarioLogueadoResultado.Errores);

            var usuarioLogueado = usuarioLogueadoResultado.Valor;
            
            if(usuarioLogueado == null ) return Resultado<(string, string, string)>.Failure(ErroresUsuario.UsuarioInexistente("Login"));

            var (accessToken, refreshToken) = await _provedorJwt.GenerarTokens(usuarioLogueado!,null!);

            usuarioLogueado.AsignarToken(accessToken, refreshToken);

            return Resultado<(string, string,string)>.Success((accessToken, refreshToken, usuarioLogueado.Id));
        }

        public async Task<Resultado<bool>> Logout()
        {
            return false;
        }

        public async Task<Resultado<(string AccessToken, string RefreshToken, string usuarioId)>> RefreshToken(string refreshToken)
        {
            var resultadoToken = await _repoRefreshToken.ObtenerPorToken(refreshToken);
            var tokenAnterior = resultadoToken.Valor;

            if (resultadoToken.TieneErrores || tokenAnterior!.TokenExpirado) return Resultado<(string, string, string)>.Failure(ErroresToken.Invalido("RefreshToken"));

            var resultadoUsuario = await _repoUsuario.ObtenerPorIdAsync(tokenAnterior.UsuarioId ?? "" );

            if (resultadoUsuario.TieneErrores) return Resultado<(string, string, string)>.Failure(resultadoUsuario.Errores);

            var usuarioLogueado = resultadoUsuario.Valor;
            if (usuarioLogueado == null) return Resultado<(string, string, string)>.Failure(ErroresUsuario.UsuarioInexistente("RefreshToken"));

            var (nuevoAccessToken, nuevoRefreshToken) = await _provedorJwt.GenerarTokens(usuarioLogueado, tokenAnterior);

            //await _repoRefreshToken.RevocarYCrearNuevo(tokenAnterior, nuevoRefreshToken);

            return Resultado<(string, string, string)>.Success((nuevoAccessToken, nuevoRefreshToken, usuarioLogueado.Id));
        }

        public async Task<Resultado<bool>> RevocarRefreshToken(string refreshToken)
        {
            var resultadoRefreshToken = await _repoRefreshToken.ObtenerPorToken(refreshToken);
            
            if (resultadoRefreshToken.TieneErrores || resultadoRefreshToken.Valor == null) return Resultado<bool>.Failure(ErroresToken.Invalido("RevocarRefreshToken"));

            var resultadoTokenRevocado = await _repoRefreshToken.Revocar(resultadoRefreshToken.Valor);

            if (resultadoTokenRevocado.TieneErrores) return Resultado<bool>.Failure(resultadoTokenRevocado.Errores);

            return resultadoTokenRevocado.Valor;
        }
    }
}
