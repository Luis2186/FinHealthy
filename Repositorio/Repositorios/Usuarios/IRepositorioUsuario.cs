using Dominio;
using Dominio.Usuarios;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.Usuarios
{
    public interface IRepositorioUsuario 
    {
        Task<Resultado<Usuario>> CrearAsync(Usuario model, string password, CancellationToken cancellationToken);
        Task<Resultado<Usuario>> Login(Usuario usuario, string password, CancellationToken cancellationToken);
        Task<Resultado<Usuario>> RestablecerContraseña(string email, string contraseñaVieja, string nuevaContraseña, CancellationToken cancellationToken);
        Task<Resultado<Usuario>> ObtenerPorIdAsync(string id, CancellationToken cancellationToken);
        Task<Resultado<bool>> EliminarAsync(string id, CancellationToken cancellationToken);
        Task<Resultado<bool>> InhabilitarUsuarioAsync(string id, CancellationToken cancellationToken);
        Task<Resultado<Usuario>> ObtenerPorEmailAsync(string email, CancellationToken cancellationToken);
        Task<Resultado<bool>> AgregarRol(string usuarioId, string idRol, string nombreRol, CancellationToken cancellationToken);
        Task<Resultado<IdentityRole>> BuscarRol(string rolId, string rolNombre, CancellationToken cancellationToken);
        Task<Resultado<bool>> RemoverRol(string usuarioId, string idRol, string nombreRol, CancellationToken cancellationToken);
        Task<Resultado<bool>> AgregarClaim(string usuarioId, string tipoClaim, string claim, CancellationToken cancellationToken);
        Task<Resultado<bool>> RemoverClaim(string usuarioId, string tipoClaim, string claim, CancellationToken cancellationToken);
        Task<Resultado<IEnumerable<Claim>>> ObtenerTodosLosClaim(string usuarioId, CancellationToken cancellationToken);
        Task<Resultado<IEnumerable<string>>> ObtenerTodosLosRoles(CancellationToken cancellationToken);
        Task<Resultado<IEnumerable<string>>> ObtenerRolesPorUsuario(string usuarioId, CancellationToken cancellationToken);
        Task<Resultado<IEnumerable<Usuario>>> ObtenerTodosAsync(CancellationToken cancellationToken);
        Task<Resultado<IEnumerable<Usuario>>> BuscarUsuarios(List<string> usuariosIds, CancellationToken cancellationToken);
        Task<Resultado<Usuario>> ActualizarAsync(Usuario usuario, CancellationToken cancellationToken);
    }
}
