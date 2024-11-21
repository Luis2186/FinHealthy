using Dominio.Usuarios;
using Microsoft.AspNetCore.Identity;
using Servicio.Usuarios.UsuariosDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.Usuarios
{
    public interface IServicioUsuario
    {
        public Task<Usuario> ObtenerPorId(string id);
        // Obtener todos los elementos con soporte de paginación y cancelación
        public Task<IEnumerable<Usuario>> ObtenerTodos();
        public Task<Usuario> Registrar(CrearUsuarioDTO usuario);
        public Task<Usuario> Login(UsuarioLoginDTO usuario);
        public Task<bool> Logout();
        public Task<Usuario> Actualizar(string id, ActualizarUsuarioDTO usuario);
        public Task<bool> Eliminar(string id);
        public Task<bool> AgregarRol(string usuarioId, string rol);
        public Task<bool> RemoverRol(string usuarioId, string rol);
        public Task<bool> AgregarClaim(string usuarioId, string tipoClaim, string claim);
        public Task<bool> RemoverClaim(string usuarioId, string tipoClaim, string claim);
        public Task<IEnumerable<Claim>> ObtenerTodosLosClaim(string usuarioId);
        public Task<IEnumerable<string>> ObtenerTodosLosRoles();
        public Task<IEnumerable<string>> ObtenerRolesPorUsuario(string usuarioId);
    }
}
