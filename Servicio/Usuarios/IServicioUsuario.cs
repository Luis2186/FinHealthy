using Dominio;
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
        public Task<Resultado<Usuario>> ObtenerPorId(string id);
        // Obtener todos los elementos con soporte de paginación y cancelación
        public Task<Resultado<IEnumerable<Usuario>>> ObtenerTodos();
        public Task<Resultado<Usuario>> Registrar(CrearUsuarioDTO usuario);
        public Task<Resultado<Usuario>> Login(UsuarioLoginDTO usuario);
        public Task<Resultado<bool>> Logout();
        public Task<Resultado<Usuario>> Actualizar(string id, ActualizarUsuarioDTO usuario);
        public Task<Resultado<bool>> Eliminar(string id);
        public Task<Resultado<bool>> AgregarRol(string usuarioId, string idRol,string nombreRol);
        public Task<Resultado<bool>> RemoverRol(string usuarioId, string rol, string nombreRol);
        public Task<Resultado<bool>> AgregarClaim(string usuarioId, string tipoClaim, string claim);
        public Task<Resultado<bool>> RemoverClaim(string usuarioId, string tipoClaim, string claim);
        public Task<Resultado<IEnumerable<Claim>>> ObtenerTodosLosClaim(string usuarioId);
        public Task<Resultado<IEnumerable<string>>> ObtenerTodosLosRoles();
        public Task<Resultado<IEnumerable<string>>> ObtenerRolesPorUsuario(string usuarioId);
    }
}
