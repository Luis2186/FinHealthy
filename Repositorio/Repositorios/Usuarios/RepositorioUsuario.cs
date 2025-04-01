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

        public async Task<Resultado<Usuario>> ActualizarAsync(Usuario model)
        {
            var resultadoValidacion = DataAnnotationsValidator.Validar(model);

            if (resultadoValidacion.TieneErrores) return resultadoValidacion;

            try
            {
                var resultadoUpdate = await _userManager.UpdateAsync(model);

                if (resultadoUpdate.Succeeded)
                {
                    return Resultado<Usuario>.Success(model); 
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

        public async Task<Resultado<bool>> AgregarClaim(string usuarioId, string tipoClaim, string claim)
        {
      
            var usuarioBuscadoResultado = await ObtenerPorIdAsync(usuarioId);

            if (usuarioBuscadoResultado.TieneErrores) return Resultado<bool>.Failure(usuarioBuscadoResultado.Errores);

            var usuarioBuscado = usuarioBuscadoResultado.Valor;
            Claim claimParaAgregar = new Claim(tipoClaim, claim);

            try
            {
                var resultado = await _userManager.AddClaimAsync(usuarioBuscado, claimParaAgregar);
                if (resultado.Succeeded)
                {
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

        public async Task<Resultado<bool>> AgregarRol(string usuarioId, string idRol, string nombreRol)
        {
            var usuarioBuscadoResultado = await ObtenerPorIdAsync(usuarioId);

            if (usuarioBuscadoResultado.TieneErrores)
            {
                return Resultado<bool>.Failure(usuarioBuscadoResultado.Errores);
            }

            var usuarioBuscado = usuarioBuscadoResultado.Valor;

            var role = await BuscarRol(idRol, nombreRol);

            if (role.TieneErrores) return Resultado<bool>.Failure(role.Errores);

            var rolEncontrado = role.Valor;

            var resultado = await _userManager.AddToRoleAsync(usuarioBuscado, rolEncontrado.Name);

            if (resultado.Succeeded) return Resultado<bool>.Success(resultado.Succeeded);

            var errores = resultado.Errors
                .Select(e => new Error(e.Code, e.Description))
                .ToList();

            return Resultado<bool>.Failure(errores);
        }

        public async Task<Resultado<IdentityRole>> BuscarRol(string rolId, string rolNombre)
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

        public async Task<Resultado<Usuario>> CrearAsync(Usuario model)
        {
            throw new NotImplementedException();
        }

        public async Task<Resultado<Usuario>> CrearAsync(Usuario model, string password)
        {
            var resultadoValidacion = DataAnnotationsValidator.Validar(model);

            if (resultadoValidacion.TieneErrores) return resultadoValidacion;

            try
            {
                var usuarioBuscado= await ObtenerPorEmailAsync(model.Email);

                if (usuarioBuscado.EsCorrecto) return Resultado<Usuario>.Failure(ErroresUsuario.EmailExistente("CrearAsync"));

                var usuarioCreado = await _userManager.CreateAsync(model, password);
               
                if (usuarioCreado.Succeeded)
                {
                    return Resultado<Usuario>.Success(model);
                }else
                {
                    var errores = usuarioCreado.Errors
                        .Select(e => new Error(e.Code,e.Description))
                        .ToList();
                    return Resultado<Usuario>.Failure(errores);
                }
            }
            catch (Exception ex)
            {
                return Resultado<Usuario>.Failure(new Error("CREACION_ERROR",ex.Message));
            }
        }

        public async Task<Resultado<bool>> EliminarAsync(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);

            if (usuario == null) return Resultado<bool>.Failure(ErroresUsuario.IdInexistente("EliminarAsync"));

            try
            {
                var usuarioEliminado = await _userManager.DeleteAsync(usuario);
                if (usuarioEliminado.Succeeded)
                {
                    return Resultado<bool>.Success(usuarioEliminado.Succeeded);
                } else
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

        public Task<Resultado<bool>> EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Resultado<bool>> InhabilitarUsuarioAsync(string id)
        {
            var usuarioResult = await ObtenerPorIdAsync(id);

            if (usuarioResult.TieneErrores) return Resultado<bool>.Failure(ErroresUsuario.IdInexistente("EliminarAsync"));

            var usuario = usuarioResult.Valor;

            usuario.Activo = false;

            var usuarioInhabilitado = await ActualizarAsync(usuario);

            if(usuarioInhabilitado.TieneErrores ) return Resultado<bool>.Failure(usuarioInhabilitado.Errores);

            return usuarioInhabilitado.EsCorrecto;
        }

        public async Task<Resultado<Usuario>> Login(Usuario usuario, string password)
        {
            try
            {
                if(!usuario.Activo) return Resultado<Usuario>.Failure(ErroresUsuario.UsuarioInhabilitado("Login"));
                
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

        public async Task<Resultado<Usuario>> ObtenerPorEmailAsync(string email)
        {
            try
            {
                var usuarioBuscado = await _userManager.Users
                    .Include(user => user.GrupoDeGastos)
                    .FirstOrDefaultAsync(user => user.Email == email);

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

        public async Task<Resultado<Usuario>> ObtenerPorIdAsync(string id)
        {
            try
            {
                var usuarioBuscado = await _userManager.Users
                    .Include(user=> user.GrupoDeGastos)
                    .FirstOrDefaultAsync(user=> user.Id == id);

                if (usuarioBuscado == null)
                {
                    return Resultado<Usuario>.Failure(ErroresUsuario.IdInexistente("ObtenerPorIdAsync"));
                }

                // Obtener los roles del usuario
                var roles = await _userManager.GetRolesAsync(usuarioBuscado);
                usuarioBuscado.Roles = roles.ToList();

                

                return Resultado<Usuario>.Success(usuarioBuscado);
            }
            catch (Exception ex)
            {
                return Resultado<Usuario>.Failure(new Error("OBTENER_USUARIO_ERROR", $"Ocurrió un error al intentar obtener el usuario: {ex.Message}"));
            }
        }

        public Task<Resultado<Usuario>> ObtenerPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Resultado<IEnumerable<string>>> ObtenerRolesPorUsuario(string usuarioId)
        {
            var usuarioBuscadoResultado = await ObtenerPorIdAsync(usuarioId);

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

        public async Task<Resultado<IEnumerable<Usuario>>> ObtenerTodosAsync()
        {
            try
            {
                var usuarios = await _userManager.Users
                    .Where(user => user.UserName.ToLower() != "sys_adm")
                    .ToListAsync();

                return Resultado<IEnumerable<Usuario>>.Success(usuarios);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<Usuario>>.Failure(new Error("OBTENER_TODOS_ERROR", $"Ocurrió un error al intentar obtener los usuarios: {ex.Message}"));
            }
        }

        public async Task<Resultado<IEnumerable<Claim>>> ObtenerTodosLosClaim(string usuarioId)
        {
            var usuarioBuscado = await ObtenerPorIdAsync(usuarioId);

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

        public async Task<Resultado<IEnumerable<string>>> ObtenerTodosLosRoles()
        {
            try
            {
                var roles = await _roleManager.Roles
                    .Select(r => r.Name)
                    .ToListAsync();

                if (!roles.Any()) return Resultado<IEnumerable<string>>.Failure(ErroresUsuario.UsuarioSinRoles("ObtenerTodosLosRoles"));

                return Resultado<IEnumerable<string>>.Success(roles);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<string>>.Failure(new Error("OBTENER_ROLES_ERROR", $"Ocurrió un error al intentar obtener los roles: {ex.Message}"));
            }
        }

        public async Task<Resultado<bool>> RemoverClaim(string usuarioId, string tipoClaim, string claim)
        {
            var usuarioBuscado = await ObtenerPorIdAsync(usuarioId);

            if (usuarioBuscado.TieneErrores) return Resultado<bool>.Failure(usuarioBuscado.Errores);

            try
            {
                var claimParaRemover = new Claim(tipoClaim, claim);

                var resultado = await _userManager.RemoveClaimAsync(usuarioBuscado.Valor, claimParaRemover);

                if (resultado.Succeeded) return Resultado<bool>.Success(resultado.Succeeded);

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

        public async Task<Resultado<bool>> RemoverRol(string usuarioId, string idRol, string nombreRol)
        {
            var usuarioBuscado = await ObtenerPorIdAsync(usuarioId);

            if (usuarioBuscado.TieneErrores) return Resultado<bool>.Failure(usuarioBuscado.Errores);

            try
            {
                // Buscar el rol por su ID
                var role = await BuscarRol(idRol, nombreRol);

                if (role.TieneErrores) return Resultado<bool>.Failure(role.Errores);

                var rol = role.Valor;
                var resultado = await _userManager.RemoveFromRoleAsync(usuarioBuscado.Valor, rol.Name);

                if (resultado.Succeeded) return Resultado<bool>.Success(resultado.Succeeded);

                var errores = resultado.Errors
                    .Select(e => new Error(e.Code, e.Description))
                    .ToList();

                return Resultado<bool>.Failure(errores);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(new Error("REMOVER_ROL_ERROR", $"Ocurrió un error al intentar remover el rol: {ex.Message}"));
            }
        }

        public async Task<Resultado<Usuario>> RestablecerContraseña(string email, string contraseñaVieja, string nuevaContraseña)
        {
            try
            {
                var usuarioBuscado = await ObtenerPorEmailAsync(email);

                if (usuarioBuscado.TieneErrores) return Resultado<Usuario>.Failure(usuarioBuscado.Errores);
                
                var resultadoCambio = await _userManager.ChangePasswordAsync(usuarioBuscado.Valor, contraseñaVieja, nuevaContraseña);

                if (resultadoCambio.Succeeded)
                {
                    await _signInManager.SignInAsync(usuarioBuscado.Valor, isPersistent: false);
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




    }
}
