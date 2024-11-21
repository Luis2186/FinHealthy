using AutoMapper;
using Dominio.Usuarios;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Repositorio.Repositorios.Usuarios;
using Servicio.Authentication;
using Servicio.Usuarios.UsuariosDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.Usuarios
{
    public class ServicioUsuario : IServicioUsuario
    {
        private readonly IMapper _mapper;
        private readonly IRepositorioUsuario _repoUsuario;
        private readonly ProveedorToken _provedorJwt;

        public ServicioUsuario(IRepositorioUsuario repositorioUsuario, IMapper mapper, ProveedorToken provedorJwt)
        {
            _mapper = mapper;
            _repoUsuario = repositorioUsuario;
            _provedorJwt = provedorJwt;
        }
        public async Task<Usuario> Actualizar(string id, ActualizarUsuarioDTO usuarioDto)
        {
            Usuario usuarioBuscado = await ObtenerPorId(id);

            if (usuarioBuscado == null) throw new Exception("El usuario que intenta actualizar no existe");

            // Mapeo de los datos del DTO al usuario existente
            _mapper.Map(usuarioDto, usuarioBuscado); // Actualizar el usuario con el DTO

            bool usuarioActualizado = await _repoUsuario.ActualizarAsync(usuarioBuscado);
            
            if (usuarioActualizado) return usuarioBuscado;
            
            return null;
        }

        public async Task<bool> AgregarClaim(string usuarioId, string tipoClaim, string claim)
        {
            return await _repoUsuario.AgregarClaim(usuarioId, tipoClaim, claim);
        }

        public async Task<bool> AgregarRol(string usuarioId, string idRol)
        {
            return await _repoUsuario.AgregarRol(usuarioId, idRol);
        }

        public async Task<Usuario> Registrar(CrearUsuarioDTO usuarioDto)
        {
            Usuario usuario = _mapper.Map<Usuario>(usuarioDto);
            bool usuarioCreado = await _repoUsuario.CrearAsync(usuario, usuarioDto.Password);
            
            if (!usuarioCreado) throw new InvalidOperationException("El usuario no se ah podido crear,revise los campos proporcionados");

            if (usuarioDto.Rol.Trim() != "")
            {
                var rol = await _repoUsuario.BuscarRol("", usuarioDto.Rol.Trim());
                if(rol.Name != null) await _repoUsuario.AgregarRol(usuario.Id, rol.Id);
            } 

            var rolesUsuario = await ObtenerRolesPorUsuario(usuario.Id);
            var listaRolesUsuarios = rolesUsuario.ToList();

            var token = _provedorJwt.Crear(usuario, rolesUsuario.ToList());

            usuario.AsignarRoles(listaRolesUsuarios);
            usuario.AsignarToken(token);

            if (usuarioCreado) return usuario;
            
            return null;
        }

        public async Task<bool> Eliminar(string id)
        {
           return await _repoUsuario.EliminarAsync(id);
        }

        public async Task<Usuario> ObtenerPorId(string id)
        {
            return await _repoUsuario.ObtenerPorIdAsync(id);
        }

        public async Task<IEnumerable<string>> ObtenerRolesPorUsuario(string usuarioId)
        {
            return await _repoUsuario.ObtenerRolesPorUsuario(usuarioId);
        }

        public async Task<IEnumerable<Usuario>> ObtenerTodos()
        {
           return await _repoUsuario.ObtenerTodosAsync();
        }

        public async Task<IEnumerable<Claim>> ObtenerTodosLosClaim(string usuarioId)
        {
            return await _repoUsuario.ObtenerTodosLosClaim(usuarioId);
        }

        public async Task<IEnumerable<string>> ObtenerTodosLosRoles()
        {
            return await _repoUsuario.ObtenerTodosLosRoles();
        }

        public async Task<bool> RemoverClaim(string usuarioId, string tipoClaim, string claim)
        {
            return await _repoUsuario.RemoverClaim(usuarioId, tipoClaim, claim);
        }

        public async Task<bool> RemoverRol(string usuarioId, string idRol)
        {
            return await _repoUsuario.RemoverRol(usuarioId, idRol);
        }

        public async Task<Usuario> Login(UsuarioLoginDTO usuario)
        {
            Usuario usuarioBuscado = await _repoUsuario.ObtenerPorEmailAsync(usuario.Email);

            bool usuarioLogueado = await _repoUsuario.Login(usuarioBuscado, usuario.Password);

            if(!usuarioLogueado) throw new KeyNotFoundException("Credenciales incorrecectas,reviselas por favor");

            var rolesUsuario = await ObtenerRolesPorUsuario(usuarioBuscado.Id);
            var listaRolesUsuarios = rolesUsuario.ToList();

            var token = _provedorJwt.Crear(usuarioBuscado, rolesUsuario.ToList());

            usuarioBuscado.AsignarRoles(listaRolesUsuarios);
            usuarioBuscado.AsignarToken(token);

            if (usuarioLogueado) return usuarioBuscado;

            return null;
        }

        public async Task<bool> Logout()
        {
            return false;
        }
    }
}
