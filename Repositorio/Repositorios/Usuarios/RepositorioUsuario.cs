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
        
        public RepositorioUsuario(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager, SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<bool> ActualizarAsync(Usuario model)
        {
            var resultado = await _userManager.UpdateAsync(model);
            return resultado.Succeeded;
        }

        public async Task<bool> AgregarClaim(string usuarioId, string tipoClaim, string claim)
        {
            Usuario usuarioBuscado = await ObtenerPorIdAsync(usuarioId);
            Claim claimParaAgregar = new Claim(tipoClaim, claim);

            if (usuarioBuscado == null) return false;

            var resultado = await _userManager.AddClaimAsync(usuarioBuscado, claimParaAgregar);

            return resultado.Succeeded;
        }

        public async Task<bool> AgregarRol(string usuarioId, string idRol)
        {
            Usuario usuarioBuscado = await ObtenerPorIdAsync(usuarioId);
            var role = await _roleManager.FindByIdAsync(idRol);

            if (usuarioBuscado == null || role == null) return false;

            var resultado = await _userManager.AddToRoleAsync(usuarioBuscado, role.Name);

            return resultado.Succeeded;
        }

        public async Task<IdentityRole> BuscarRol(string rolId, string rolNombre)
        {
            IdentityRole rolBuscado = await _roleManager.FindByIdAsync(rolId);

            if(rolBuscado == null) rolBuscado = await _roleManager.FindByNameAsync(rolNombre);

            return rolBuscado;
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

        public async Task<bool> Login(Usuario usuario, string password)
        {
            return await _userManager.CheckPasswordAsync(usuario, password);
        }

        public async Task<Usuario> ObtenerPorEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<Usuario> ObtenerPorIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public Task<Usuario> ObtenerPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> ObtenerRolesPorUsuario(string usuarioId)
        {
            Usuario usuarioBuscado = await ObtenerPorIdAsync(usuarioId);
            if (usuarioBuscado == null) return null;

            return await _userManager.GetRolesAsync(usuarioBuscado);
        }

        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            return _userManager.Users.ToList();
        }

        public async Task<IEnumerable<Claim>> ObtenerTodosLosClaim(string usuarioId)
        {
            Usuario usuarioBuscado = await ObtenerPorIdAsync(usuarioId);

            if (usuarioBuscado == null) return null;

            return await _userManager.GetClaimsAsync(usuarioBuscado);
        }

        public async Task<IEnumerable<string>> ObtenerTodosLosRoles()
        {
            return await _roleManager.Roles
                .Select(r => r.Name)
                .ToListAsync();
        }

        public async Task<bool> RemoverClaim(string usuarioId, string tipoClaim, string claim)
        {
            Usuario usuarioBuscado = await ObtenerPorIdAsync(usuarioId);
            Claim claimParaRemover = new Claim(tipoClaim, claim);

            if (usuarioBuscado == null) return false;

            var resultado = await _userManager.RemoveClaimAsync(usuarioBuscado, claimParaRemover);

            return resultado.Succeeded;
        }

        public async Task<bool> RemoverRol(string usuarioId, string idRol)
        {
            Usuario usuarioBuscado = await ObtenerPorIdAsync(usuarioId);
            var role = await _roleManager.FindByIdAsync(idRol);

            if (usuarioBuscado == null || role == null) return false;

            var resultado = await _userManager.RemoveFromRoleAsync(usuarioBuscado, role.Name);

            return resultado.Succeeded;
        }

        public async Task<Usuario> RestablecerContraseña(string email, string contraseñaVieja, string nuevaContraseña)
        {
            var user = await ObtenerPorEmailAsync(email);

            if (user != null)
            {
                var resultado =  await _userManager.ChangePasswordAsync(user, contraseñaVieja, nuevaContraseña);
                
                if (resultado.Succeeded)
                {
                    // Inicia sesión con la nueva contraseña
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    
                    return user;
                }
            }

            return null;
        }




    }
}
