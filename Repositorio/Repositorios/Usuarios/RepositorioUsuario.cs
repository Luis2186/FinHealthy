using Dominio;
using Dominio.Abstracciones;
using Dominio.Usuarios;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.Usuarios
{
    public class RepositorioUsuario : IRepositorioUsuario
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly ApplicationDbContext _dbContext;

        public RepositorioUsuario(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager,
            SignInManager<Usuario> signInManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }

        public async Task<Resultado<Usuario>> ActualizarAsync(Usuario usuario, CancellationToken cancellationToken)
        {
            var resultadoValidacion = DataAnnotationsValidator.Validar(usuario);
            if (resultadoValidacion.TieneErrores) return resultadoValidacion;
            try
            {
                var resultadoUpdate = await _userManager.UpdateAsync(usuario);
                if (resultadoUpdate.Succeeded)
                {
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    return Resultado<Usuario>.Success(usuario);
                }
                else
                {
                    var errores = resultadoUpdate.Errors
                        .Select(e => new Error(e.Code, e.Description))
                        .ToList();
                    return Resultado<Usuario>.Failure(errores);
                }
            }
            catch (Exception ex)
            {
                return Resultado<Usuario>.Failure(new Error("ACTUALIZACION_ERROR", $"Error inesperado al actualizar el usuario: {ex.Message}"));
            }
        }

        public async Task<Resultado<Usuario>> CrearAsync(Usuario model, string password, CancellationToken cancellationToken)
        {
            var resultadoValidacion = DataAnnotationsValidator.Validar(model);
            if (resultadoValidacion.TieneErrores) return resultadoValidacion;
            try
            {
                var usuarioBuscado = await ObtenerPorEmailAsync(model.Email, cancellationToken);
                if (usuarioBuscado.EsCorrecto) return Resultado<Usuario>.Failure(ErroresUsuario.EmailExistente("CrearAsync"));
                var usuarioCreado = await _userManager.CreateAsync(model, password);
                if (usuarioCreado.Succeeded)
                {
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    return Resultado<Usuario>.Success(model);
                }
                else
                {
                    var errores = usuarioCreado.Errors
                        .Select(e => new Error(e.Code, e.Description))
                        .ToList();
                    return Resultado<Usuario>.Failure(errores);
                }
            }
            catch (Exception ex)
            {
                return Resultado<Usuario>.Failure(new Error("CREACION_ERROR", ex.Message));
            }
        }

        public async Task<Resultado<bool>> EliminarAsync(string id, CancellationToken cancellationToken)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null) return Resultado<bool>.Failure(ErroresUsuario.IdInexistente("EliminarAsync"));
            try
            {
                var usuarioEliminado = await _userManager.DeleteAsync(usuario);
                if (usuarioEliminado.Succeeded)
                {
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    return Resultado<bool>.Success(usuarioEliminado.Succeeded);
                }
                else
                {
                    var errores = usuarioEliminado.Errors
                        .Select(error => new Error(error.Code, error.Description))
                        .ToList();
                    return Resultado<bool>.Failure(errores);
                }
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(new Error("ELIMINACION_ERROR", ex.Message));
            }
        }

        public async Task<Resultado<bool>> InhabilitarUsuarioAsync(string id, CancellationToken cancellationToken)
        {
            var usuarioResult = await ObtenerPorIdAsync(id, cancellationToken);
            if (usuarioResult.TieneErrores) return Resultado<bool>.Failure(ErroresUsuario.IdInexistente("EliminarAsync"));
            var usuario = usuarioResult.Valor;
            usuario.Activo = false;
            var usuarioInhabilitado = await ActualizarAsync(usuario, cancellationToken);
            if (usuarioInhabilitado.TieneErrores) return Resultado<bool>.Failure(usuarioInhabilitado.Errores);
            return usuarioInhabilitado.EsCorrecto;
        }

        public async Task<Resultado<Usuario>> Login(Usuario usuario, string password, CancellationToken cancellationToken)
        {
            try
            {
                if (!usuario.Activo) return Resultado<Usuario>.Failure(ErroresUsuario.UsuarioInhabilitado("Login"));
                var usuarioLogueado = await _userManager.CheckPasswordAsync(usuario, password);
                if (usuarioLogueado)
                {
                    return Resultado<Usuario>.Success(usuario);
                }
                return Resultado<Usuario>.Failure(ErroresUsuario.CredencialesInvalidas("Login"));
            }
            catch (Exception ex)
            {
                return Resultado<Usuario>.Failure(new Error("LOGIN_ERROR", $"Ocurrió un error al intentar iniciar sesión: {ex.Message}"));
            }
        }

        public async Task<Resultado<Usuario>> RestablecerContraseña(string email, string contraseñaVieja, string nuevaContraseña, CancellationToken cancellationToken)
        {
            try
            {
                var usuarioBuscado = await ObtenerPorEmailAsync(email, cancellationToken);
                if (usuarioBuscado.TieneErrores) return Resultado<Usuario>.Failure(usuarioBuscado.Errores);
                var resultadoCambio = await _userManager.ChangePasswordAsync(usuarioBuscado.Valor, contraseñaVieja, nuevaContraseña);
                if (resultadoCambio.Succeeded)
                {
                    await _signInManager.SignInAsync(usuarioBuscado.Valor, isPersistent: false);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    return Resultado<Usuario>.Success(usuarioBuscado.Valor);
                }
                var errores = resultadoCambio.Errors
                    .Select(e => new Error(e.Code, e.Description))
                    .ToList();
                return Resultado<Usuario>.Failure(errores);
            }
            catch (Exception ex)
            {
                return Resultado<Usuario>.Failure(new Error("RESTABLECER_CONTRASEÑA_ERROR", $"Ocurrió un error al intentar restablecer la contraseña: {ex.Message}"));
            }
        }

        public async Task<Resultado<Usuario>> ObtenerPorEmailAsync(string email, CancellationToken cancellationToken)
        {
            try
            {
                var usuarioBuscado = await _userManager.Users
                    .Include(user => user.GrupoDeGastos)
                    .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
                if (usuarioBuscado == null)
                {
                    return Resultado<Usuario>.Failure(ErroresUsuario.EmailInexistente("ObtenerPorEmailAsync"));
                }
                return Resultado<Usuario>.Success(usuarioBuscado);
            }
            catch (Exception ex)
            {
                return Resultado<Usuario>.Failure(new Error("OBTENER_USUARIO_ERROR", $"Ocurrió un error al intentar obtener el usuario: {ex.Message}"));
            }
        }

        public async Task<Resultado<Usuario>> ObtenerPorIdAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                var usuarioBuscado = await _userManager.Users
                    .Include(user => user.GrupoDeGastos)
                    .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
                if (usuarioBuscado == null)
                {
                    return Resultado<Usuario>.Failure(ErroresUsuario.IdInexistente("ObtenerPorIdAsync"));
                }
                var roles = await _userManager.GetRolesAsync(usuarioBuscado);
                usuarioBuscado.Roles = roles.ToList();
                return Resultado<Usuario>.Success(usuarioBuscado);
            }
            catch (Exception ex)
            {
                return Resultado<Usuario>.Failure(new Error("OBTENER_USUARIO_ERROR", $"Ocurrió un error al intentar obtener el usuario: {ex.Message}"));
            }
        }
        public async Task<Resultado<IEnumerable<Usuario>>> ObtenerTodosAsync(CancellationToken cancellationToken)
        {
            try
            {
                var usuarios = await _userManager.Users
                    .Where(user => user.UserName.ToLower() != "sys_adm")
                    .ToListAsync(cancellationToken);
                return Resultado<IEnumerable<Usuario>>.Success(usuarios);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<Usuario>>.Failure(new Error("OBTENER_TODOS_ERROR", $"Ocurrió un error al intentar obtener los usuarios: {ex.Message}"));
            }
        }

        public async Task<Resultado<bool>> AgregarClaim(string usuarioId, string tipoClaim, string claim, CancellationToken cancellationToken)
        {
            var usuarioBuscadoResultado = await ObtenerPorIdAsync(usuarioId, cancellationToken);
            if (usuarioBuscadoResultado.TieneErrores) return Resultado<bool>.Failure(usuarioBuscadoResultado.Errores);
            var usuarioBuscado = usuarioBuscadoResultado.Valor;
            Claim claimParaAgregar = new Claim(tipoClaim, claim);
            try
            {
                var resultado = await _userManager.AddClaimAsync(usuarioBuscado, claimParaAgregar);
                if (resultado.Succeeded)
                {
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    return Resultado<bool>.Success(resultado.Succeeded);
                }
                else
                {
                    var errores = resultado.Errors
                        .Select(e => new Error(e.Code, e.Description))
                        .ToList();
                    return Resultado<bool>.Failure(errores);
                }
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(new Error("CREACION_ERROR", $"Error inesperado al agregar el claim al usuario: {ex.Message}"));
            }
        }

        public async Task<Resultado<bool>> AgregarRol(string usuarioId, string idRol, string nombreRol, CancellationToken cancellationToken)
        {
            var usuarioBuscadoResultado = await ObtenerPorIdAsync(usuarioId, cancellationToken);
            if (usuarioBuscadoResultado.TieneErrores)
            {
                return Resultado<bool>.Failure(usuarioBuscadoResultado.Errores);
            }
            var usuarioBuscado = usuarioBuscadoResultado.Valor;
            var role = await BuscarRol(idRol, nombreRol, cancellationToken);
            if (role.TieneErrores) return Resultado<bool>.Failure(role.Errores);
            var rolEncontrado = role.Valor;
            var resultado = await _userManager.AddToRoleAsync(usuarioBuscado, rolEncontrado.Name);
            if (resultado.Succeeded) return Resultado<bool>.Success(resultado.Succeeded);
            var errores = resultado.Errors
                .Select(e => new Error(e.Code, e.Description))
                .ToList();
            return Resultado<bool>.Failure(errores);
        }

        public async Task<Resultado<IdentityRole>> BuscarRol(string rolId, string rolNombre, CancellationToken cancellationToken)
        {
            try
            {
                var rolBuscado = await _roleManager.FindByIdAsync(rolId);
                if (rolBuscado == null) rolBuscado = await _roleManager.FindByNameAsync(rolNombre);
                if (rolBuscado == null) return Resultado<IdentityRole>.Failure(ErroresUsuario.RolNoEncontrado("BuscarRol"));
                return Resultado<IdentityRole>.Success(rolBuscado);
            }
            catch (Exception ex)
            {
                return Resultado<IdentityRole>.Failure(new Error("BUSCAR_ROL_ERROR", $"Ocurrió un error al intentar buscar el rol: {ex.Message}"));
            }
        }

        public async Task<Resultado<bool>> RemoverRol(string usuarioId, string idRol, string nombreRol, CancellationToken cancellationToken)
        {
            var usuarioBuscado = await ObtenerPorIdAsync(usuarioId, cancellationToken);
            if (usuarioBuscado.TieneErrores) return Resultado<bool>.Failure(usuarioBuscado.Errores);
            var role = await BuscarRol(idRol, nombreRol, cancellationToken);
            if (role.TieneErrores) return Resultado<bool>.Failure(role.Errores);
            var rol = role.Valor;
            var resultado = await _userManager.RemoveFromRoleAsync(usuarioBuscado.Valor, rol.Name);
            if (resultado.Succeeded) return Resultado<bool>.Success(resultado.Succeeded);
            var errores = resultado.Errors
                .Select(e => new Error(e.Code, e.Description))
                .ToList();
            return Resultado<bool>.Failure(errores);
        }

        public async Task<Resultado<bool>> RemoverClaim(string usuarioId, string tipoClaim, string claim, CancellationToken cancellationToken)
        {
            var usuarioBuscado = await ObtenerPorIdAsync(usuarioId, cancellationToken);
            if (usuarioBuscado.TieneErrores) return Resultado<bool>.Failure(usuarioBuscado.Errores);
            try
            {
                var claimParaRemover = new Claim(tipoClaim, claim);
                var resultado = await _userManager.RemoveClaimAsync(usuarioBuscado.Valor, claimParaRemover);
                if (resultado.Succeeded)
                {
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    return Resultado<bool>.Success(resultado.Succeeded);
                }
                var errores = resultado.Errors
                    .Select(e => new Error(e.Code, e.Description))
                    .ToList();
                return Resultado<bool>.Failure(errores);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(new Error("REMOVER_CLAIM_ERROR", $"Ocurrió un error al intentar remover el claim: {ex.Message}"));
            }
        }

        public async Task<Resultado<IEnumerable<Claim>>> ObtenerTodosLosClaim(string usuarioId, CancellationToken cancellationToken)
        {
            var usuarioBuscado = await ObtenerPorIdAsync(usuarioId, cancellationToken);
            if (usuarioBuscado.TieneErrores) return Resultado<IEnumerable<Claim>>.Failure(usuarioBuscado.Errores);
            try
            {
                var claims = await _userManager.GetClaimsAsync(usuarioBuscado.Valor);
                if (!claims.Any()) return Resultado<IEnumerable<Claim>>.Failure(ErroresUsuario.UsuarioSinClaims("ObtenerTodosLosClaim"));
                return Resultado<IEnumerable<Claim>>.Success(claims);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<Claim>>.Failure(new Error("OBTENER_CLAIMS_ERROR", $"Ocurrió un error al intentar obtener los claims: {ex.Message}"));
            }
        }

        public async Task<Resultado<IEnumerable<string>>> ObtenerTodosLosRoles(CancellationToken cancellationToken)
        {
            try
            {
                var roles = await _roleManager.Roles
                    .Select(r => r.Name)
                    .ToListAsync(cancellationToken);
                if (!roles.Any()) return Resultado<IEnumerable<string>>.Failure(ErroresUsuario.UsuarioSinRoles("ObtenerTodosLosRoles"));
                return Resultado<IEnumerable<string>>.Success(roles);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<string>>.Failure(new Error("OBTENER_ROLES_ERROR", $"Ocurrió un error al intentar obtener los roles: {ex.Message}"));
            }
        }

        public async Task<Resultado<IEnumerable<string>>> ObtenerRolesPorUsuario(string usuarioId, CancellationToken cancellationToken)
        {
            var usuarioBuscadoResultado = await ObtenerPorIdAsync(usuarioId, cancellationToken);
            if (usuarioBuscadoResultado.TieneErrores) return Resultado<IEnumerable<string>>.Failure(usuarioBuscadoResultado.Errores);
            var usuarioBuscado = usuarioBuscadoResultado.Valor;
            try
            {
                var roles = await _userManager.GetRolesAsync(usuarioBuscado);
                return Resultado<IEnumerable<string>>.Success(roles);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<string>>.Failure(new Error("OBTENER_ROLES_ERROR", $"Ocurrió un error al intentar obtener los roles: {ex.Message}"));
            }
        }

        public async Task<Resultado<IEnumerable<Usuario>>> BuscarUsuarios(List<string> usuariosIds, CancellationToken cancellationToken)
        {
            List<Usuario> usuarios = new List<Usuario>();
            foreach (var usuarioId in usuariosIds)
            {
                var usuarioResult = await ObtenerPorIdAsync(usuarioId, cancellationToken);
                if (usuarioResult.TieneErrores) return Resultado<IEnumerable<Usuario>>.Failure(usuarioResult.Errores);
                if (usuarioResult.Valor != null) usuarios.Add(usuarioResult.Valor);
            }
            return Resultado<IEnumerable<Usuario>>.Success(usuarios);
        }
    }
}
