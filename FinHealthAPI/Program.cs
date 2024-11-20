using Dominio.Usuarios;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositorio;
using FinHealthAPI.Extensiones;
using Repositorio.Repositorios.Usuarios;
using Servicio.Automapper;
using Servicio.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FinHealthAPI.ConfigOpcionesJwt;
using FinHealthAPI.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer()
    .AddCookie(IdentityConstants.ApplicationScheme);

builder.Services.ConfigureOptions<ConfigJwtOpciones>();
builder.Services.ConfigureOptions<ConfigBearerOpciones>();

builder.Services.AddControllers();

builder.Services.AddIdentityCore<Usuario>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Db_Local"),
    b => b.MigrationsAssembly("Repositorio"))
    

);

// Configuración de AutoMapper
builder.Services.AddAutoMapper(typeof(PerfilDeMapeo));

/* Inyeccion de dependencias*/
builder.Services.AddScoped<IProvedorJwt, ProvedorJwt>();
builder.Services.AddScoped<IServicioUsuario, ServicioUsuario>();
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();

var app = builder.Build();

await app.CrearRoles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.AplicarMigraciones();
}


// Configure the HTTP request pipeline.


app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.MapIdentityApi<Usuario>();
app.UseHttpsRedirection();
app.Run();
