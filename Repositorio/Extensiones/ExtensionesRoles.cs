using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Dominio.Usuarios;

namespace Repositorio.Extensiones
{
    public static class ExtensionesRoles
    {
        public async static Task CrearRoles(this IApplicationBuilder app)
        {
            using (IServiceScope scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<Usuario>>();

                // Crear roles si no existen
                var roles = new[] { "Sys_Adm", "Usuario", "Administrador" };

                foreach (var role in roles)
                {
                    var roleExist = await roleManager.RoleExistsAsync(role);
                    if (!roleExist)
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                Usuario usuarioSys_Adm = new Usuario()
                {
                    UserName = "sys_adm",
                    Nombre = "sys_adm",
                    Apellido = "sys_adm",
                    Email = "l.lopezperdomo.e@gmail.com",
                    Activo = true,
                    FechaDeNacimiento= DateTime.Now,
                    FechaDeRegistro = DateTime.Now,
                };
                
                // Agregar usuarios a roles si es necesario (opcional)
                var adminUser = await userManager.FindByEmailAsync("l.lopezperdomo.e@gmail.com");
                
                if (adminUser == null)
                {
                    var resultado = await userManager.CreateAsync(usuarioSys_Adm, "Admin_123456!");

                    if (resultado.Succeeded)
                    {
                        await userManager.AddToRoleAsync(usuarioSys_Adm, "Sys_Adm");
                    }
                }

                if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Sys_Adm");
                }               
            }
        }
    }
}
