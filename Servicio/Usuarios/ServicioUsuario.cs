using AutoMapper;
using Dominio.Usuarios;
using Repositorio.Repositorios.Usuarios;
using Servicio.Usuarios.UsuariosDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.Usuarios
{
    public class ServicioUsuario : IServicioUsuario
    {
        private readonly IMapper _mapper;
        private readonly IRepositorioUsuario _repoUsuario;

        public ServicioUsuario(IRepositorioUsuario repositorioUsuario, IMapper mapper)
        {
            _mapper = mapper;
            _repoUsuario = repositorioUsuario;
        }
        public async Task<Usuario> Actualizar(ActualizarUsuarioDTO usuarioDto)
        {
            Usuario usuario = _mapper.Map<Usuario>(usuarioDto);

            bool usuarioActualizado = await _repoUsuario.ActualizarAsync(usuario);
            
            if (usuarioActualizado) return usuario;
            
            return null;
        }

        public Task<Usuario> Actualizar(string id, ActualizarUsuarioDTO usuario)
        {
            throw new NotImplementedException();
        }

        public async Task<Usuario> Crear(CrearUsuarioDTO usuarioDto)
        {
            Usuario usuario = _mapper.Map<Usuario>(usuarioDto);
            bool usuarioCreado = await _repoUsuario.CrearAsync(usuario, usuarioDto.Password);

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
        public async Task<IEnumerable<Usuario>> ObtenerTodos()
        {
           return await _repoUsuario.ObtenerTodosAsync();
        }
    }
}
