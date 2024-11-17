using Dominio.Usuarios;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.Usuarios
{
    public class RepositorioUsuario : IRepositorioUsuario
    {
        private readonly UserManager<Usuario> _userManager;

        public RepositorioUsuario(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> ActualizarAsync(Usuario model)
        {
            var resultado = await _userManager.UpdateAsync(model);
            return resultado.Succeeded;
        }

        public async Task<bool> CrearAsync(Usuario model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CrearAsync(Usuario model, string password)
        {
            var resultado = await _userManager.CreateAsync(model, password);

            return resultado.Succeeded;
        }

        public async Task<bool> EliminarAsync(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);

            if (usuario != null)
            {
                var resultado = await _userManager.DeleteAsync(usuario);
                return resultado.Succeeded;
            }
            return false;
        }

        public async Task<Usuario> ObtenerPorIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public Task<Usuario> ObtenerPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            return _userManager.Users.ToList();
        }
    }
}
