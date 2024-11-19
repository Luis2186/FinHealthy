using Dominio.Usuarios;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.Usuarios
{
    public interface IRepositorioUsuario : IRepositorioCRUD<Usuario>
    {
        public Task<bool> CrearAsync(Usuario model,string password);
        public Task<Usuario> ObtenerPorIdAsync(string id);
        public Task<bool> AgregarRol(string usuarioId, string idRol);
        public Task<bool> RemoverRol(string usuarioId, string idRol);
        public Task<bool> AgregarClaim(string usuarioId, string tipoClaim, string claim);
        public Task<bool> RemoverClaim(string usuarioId, string tipoClaim, string claim);
        public Task<IEnumerable<Claim>> ObtenerTodosLosClaim(string usuarioId);
        public Task<IEnumerable<IdentityRole>> ObtenerTodosLosRoles();
        public Task<IEnumerable<string>> ObtenerRolesPorUsuario(string usuarioId);
    }
}
