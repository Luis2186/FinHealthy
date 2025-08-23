using Dominio;
using Dominio.Usuarios;
using Microsoft.AspNetCore.Identity;
using Servicio.DTOS.UsuariosDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Servicio.Usuarios
{
    public interface IServicioUsuario
    {
        Task<Resultado<Usuario>> ObtenerPorId(string id, CancellationToken cancellationToken);
        Task<Resultado<IEnumerable<Usuario>>> ObtenerTodos(CancellationToken cancellationToken);
        Task<Resultado<(string AccessToken, string RefreshToken, string usuarioId)>> Registrar(CrearUsuarioDTO usuario, CancellationToken cancellationToken);
        Task<Resultado<(string AccessToken, string RefreshToken, string usuarioId)>> Login(UsuarioLoginDTO usuario, CancellationToken cancellationToken);
        Task<Resultado<bool>> Logout(CancellationToken cancellationToken);
        Task<Resultado<Usuario>> Actualizar(string id, ActualizarUsuarioDTO usuario, CancellationToken cancellationToken);
        Task<Resultado<bool>> Eliminar(string id, CancellationToken cancellationToken);
        Task<Resultado<bool>> AgregarRol(string usuarioId, string idRol, string nombreRol, CancellationToken cancellationToken);
        Task<Resultado<bool>> RemoverRol(string usuarioId, string rol, string nombreRol, CancellationToken cancellationToken);
        Task<Resultado<bool>> AgregarClaim(string usuarioId, string tipoClaim, string claim, CancellationToken cancellationToken);
        Task<Resultado<bool>> RemoverClaim(string usuarioId, string tipoClaim, string claim, CancellationToken cancellationToken);
        Task<Resultado<IEnumerable<Claim>>> ObtenerTodosLosClaim(string usuarioId, CancellationToken cancellationToken);
        Task<Resultado<IEnumerable<string>>> ObtenerTodosLosRoles(CancellationToken cancellationToken);
        Task<Resultado<IEnumerable<string>>> ObtenerRolesPorUsuario(string usuarioId, CancellationToken cancellationToken);
        Task<Resultado<(string AccessToken, string RefreshToken, string usuarioId)>> RefreshToken(string refreshToken, CancellationToken cancellationToken);
        Task<Resultado<bool>> RevocarRefreshToken(string refreshToken, CancellationToken cancellationToken);
    }
}
