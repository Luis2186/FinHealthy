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
using Repositorio.Repositorios.R_Familias;
using Repositorio.Repositorios.R_Familia;
using Servicio.S_Familias;
using Repositorio.Repositorios;
using Repositorio.Repositorios.Solicitudes;
using Repositorio.Repositorios.Monedas;
using Servicio.ServiciosExternos;
using FinHealthAPI.ProcesosSegundoPlano;
using Repositorio.Repositorios.Validacion;
using Repositorio.Repositorios.R_Gastos.R_Categoria;
using Servicio.S_Gastos.S_Categoria;
using Servicio.S_Gastos;

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

// Agregar CORS con políticas personalizadas
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:4321")  // Origen de tu frontend
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddControllers();

// Configuraci�n de Identity con roles
builder.Services.AddIdentityCore<Usuario>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Db_Local"),
    b => b.MigrationsAssembly("Repositorio"))
);

// Registrar HttpClient
builder.Services.AddHttpClient();
builder.Services.AgregarInfrastructura();
// Configuraci�n de AutoMapper
builder.Services.AddAutoMapper(typeof(PerfilDeMapeo));

builder.Services.AddLogging();
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

/* Inyeccion de dependencias*/
/*Genericas*/
builder.Services.AddScoped(typeof(IValidacion<>), typeof(ValidadorDataAnnotations<>));
builder.Services.AddScoped(typeof(IRepositorioCRUD<>), typeof(RepositorioCRUD<>));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IServicioNotificacion, ServicioNotificacion>();
builder.Services.AddScoped<IServicioUsuario, ServicioUsuario>();
builder.Services.AddScoped<IServicioFamilia, ServicioFamilia>();
builder.Services.AddScoped<IServicioMonedas, ServicioMonedas>();
builder.Services.AddScoped<IServicioCategoria, ServicioCategoria>();
builder.Services.AddScoped<IServicioGasto, ServicioGasto>();

builder.Services.AddScoped<IRepositorioMoneda, RepositorioMoneda>();
builder.Services.AddScoped<IRepositorioFamilia, RepositorioFamilia>();
builder.Services.AddScoped<IRepositorioSolicitud, RepositorioSolicitud>();
builder.Services.AddScoped<IRepositorioMiembroFamilia, RepositorioMiembroFamilia>();
builder.Services.AddScoped<IRepositorioNotificacion, RepositorioNotificacion>();
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
builder.Services.AddScoped<IRepositorioCategoria, RepositorioCategoria>();



builder.Services.AddSingleton<ProveedorToken>();
var app = builder.Build();

await app.CrearRoles();
await app.PoblarDatos();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Usar CORS antes de las rutas
app.UseCors("AllowSpecificOrigin");

app.AplicarMigraciones();

// Configure the HTTP request pipeline.
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

app.MapIdentityApi<Usuario>();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.UseHttpsRedirection();
app.Run();
