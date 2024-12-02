using AutoMapper;
using Dominio;
using Dominio.Abstracciones;
using Dominio.Familias;
using Dominio.Usuarios;
using Repositorio.Repositorios;
using Repositorio.Repositorios.R_Familias;
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
        private readonly IRepositorioMiembroFamilia _repoMiembrosFamilia;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ProveedorToken _provedorJwt;

        public ServicioUsuario(IRepositorioUsuario repositorioUsuario,
            IRepositorioMiembroFamilia repositorioMiembroFamilia,
            IMapper mapper, IUnitOfWork unitOfWork, ProveedorToken provedorJwt)
        {
            _mapper = mapper;
            _repoUsuario = repositorioUsuario;
            _repoMiembrosFamilia = repositorioMiembroFamilia;
            _unitOfWork = unitOfWork;
            _provedorJwt = provedorJwt;
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

        public async Task<Resultado<Usuario>> Registrar(CrearUsuarioDTO usuarioDto)
        {
            await _unitOfWork.IniciarTransaccionAsync();

            Usuario usuario = _mapper.Map<Usuario>(usuarioDto);

            var usuarioCreado = await _repoUsuario.CrearAsync(usuario, usuarioDto.Password);
            
            if (usuarioCreado.TieneErrores) return Resultado<Usuario>.Failure(usuarioCreado.Errores);

            if (usuarioDto.Rol.Trim() != "")
            {
                var rol = await _repoUsuario.BuscarRol("", usuarioDto.Rol.Trim());

                if (rol.TieneErrores) return Resultado<Usuario>.Failure(rol.Errores);
                
                var rolesAgregados = await _repoUsuario.AgregarRol(usuario.Id,rol.Valor.Id, rol.Valor.Name);

                if(rolesAgregados.TieneErrores) return Resultado<Usuario>.Failure(rolesAgregados.Errores);
            } 

            var rolesUsuario = await ObtenerRolesPorUsuario(usuario.Id);
            var listaRolesUsuarios = rolesUsuario.Valor.ToList();

            var token = _provedorJwt.Crear(usuario, listaRolesUsuarios);

            usuario.AsignarRoles(listaRolesUsuarios);
            usuario.AsignarToken(token);

            var miembro = new MiembroFamilia();
            miembro = miembro.ConvertirUsuarioEnMiembro(usuarioCreado.Valor);
            var crearMiembro = await _repoMiembrosFamilia.CrearAsync(miembro);

            if (crearMiembro.TieneErrores)
            {
                _unitOfWork.RevertirTransaccionAsync();
                return Resultado<Usuario>.Failure(crearMiembro.Errores);
            }

            await _unitOfWork.ConfirmarTransaccionAsync();
            return usuarioCreado;
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

        public async Task<Resultado<Usuario>> Login(UsuarioLoginDTO usuario)
        {
            var usuarioBuscado = await _repoUsuario.ObtenerPorEmailAsync(usuario.Email);

            if (usuarioBuscado.TieneErrores) return Resultado<Usuario>.Failure(new Error("Usuario.Login", "El usuario y/o la contraseña son incorrectos."));

            var usuarioLogueadoResultado = await _repoUsuario.Login(usuarioBuscado.Valor, usuario.Password);

            if(usuarioLogueadoResultado.TieneErrores) return usuarioLogueadoResultado;

            var usuarioLogueado = usuarioLogueadoResultado.Valor;
            
            var rolesUsuario = await ObtenerRolesPorUsuario(usuarioLogueado.Id);
            var listaRolesUsuario = rolesUsuario.Valor.ToList();
            var token = _provedorJwt.Crear(usuarioLogueado, listaRolesUsuario);

            usuarioLogueado.AsignarRoles(listaRolesUsuario);
            usuarioLogueado.AsignarToken(token);

            return usuarioLogueado;
        }

        public async Task<Resultado<bool>> Logout()
        {
            return false;
        }
    }
}
