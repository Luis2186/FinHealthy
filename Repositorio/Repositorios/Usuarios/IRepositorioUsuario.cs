using Dominio.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.Usuarios
{
    public interface IRepositorioUsuario : IRepositorioCRUD<Usuario>
    {
        public Task<bool> CrearAsync(Usuario model,string password);
        public Task<Usuario> ObtenerPorIdAsync(string id);
    }
}
