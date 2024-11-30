using Dominio.Usuarios;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositorio;
using FinHealthAPI.Extensiones;
using Repositorio.Repositorios.Usuarios;
using Servicio.Automapper;
using Servicio.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Servicio.Authentication;
using FinHealthAPI.Middlewares;
using Servicio.Notificaciones;
using Repositorio.Repositorios.Notificaciones;
using Repositorio.Repositorios.Familia;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.TokenValidationParameters = new TokenValidationParameters()
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:ClaveSecreta"])),
            ValidIssuer = builder.Configuration["Jwt:Editor"],
            ValidAudience = builder.Configuration["Jwt:Audiencia"],
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddControllers();

// Configuración de Identity con roles
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

builder.Services.AddLogging();
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

/* Inyeccion de dependencias*/
builder.Services.AddScoped<IServicioNotificacion, ServicioNotificacion>();
builder.Services.AddScoped<IServicioUsuario, ServicioUsuario>();

builder.Services.AddScoped<IRepositorioGrupoFamilia, RepositorioGrupoFamilia>();
builder.Services.AddScoped<IRepositorioMiembroFamilia, RepositorioMiembroFamilia>();
builder.Services.AddScoped<IRepositorioNotificacion, RepositorioNotificacion>();
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();

builder.Services.AddSingleton<ProveedorToken>();
var app = builder.Build();

await app.CrearRoles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.AplicarMigraciones();
}


// Configure the HTTP request pipeline.
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

app.MapIdentityApi<Usuario>();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.UseHttpsRedirection();
app.Run();
