using Dominio;
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
        public Task<Resultado<Usuario>> CrearAsync(Usuario model,string password);
        public Task<Resultado<Usuario>> Login(Usuario usuario, string password);
        public Task<Resultado<Usuario>> RestablecerContraseña(string email, string contraseñaVieja ,string nuevaContraseña);
        public Task<Resultado<Usuario>> ObtenerPorIdAsync(string id);
        public Task<Resultado<bool>> EliminarAsync(string id);
        public Task<Resultado<Usuario>> ObtenerPorEmailAsync(string email);
        public Task<Resultado<bool>> AgregarRol(string usuarioId, string idRol,string nombreRol);
        public Task<Resultado<IdentityRole>> BuscarRol(string rolId, string rolNombre);
        public Task<Resultado<bool>> RemoverRol(string usuarioId, string idRol, string nombreRol);
        public Task<Resultado<bool>> AgregarClaim(string usuarioId, string tipoClaim, string claim);
        public Task<Resultado<bool>> RemoverClaim(string usuarioId, string tipoClaim, string claim);
        public Task<Resultado<IEnumerable<Claim>>> ObtenerTodosLosClaim(string usuarioId);
        public Task<Resultado<IEnumerable<string>>> ObtenerTodosLosRoles();
        public Task<Resultado<IEnumerable<string>>> ObtenerRolesPorUsuario(string usuarioId);
        public Task<Resultado<Usuario>> ObtenerPorIdAsync(int id);
        public Task<Resultado<IEnumerable<Usuario>>> ObtenerTodosAsync();
    }
}
