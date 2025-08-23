using Dominio.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FinHealthAPI.Extensiones;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositorio;
using Repositorio.Repositorios.Usuarios;
using Servicio.Automapper;
using Servicio.Usuarios;
using FinHealthAPI.Middlewares;
using Servicio.Notificaciones;
using Repositorio.Repositorios.Notificaciones;
using Repositorio.Repositorios;
using Repositorio.Repositorios.Solicitudes;
using Repositorio.Repositorios.R_Gastos.R_Monedas;
using Servicio.ServiciosExternos;
using FinHealthAPI.ProcesosSegundoPlano;
using Repositorio.Repositorios.Validacion;
using Servicio.S_Gastos;
using Servicio.S_Categorias;
using Repositorio.Repositorios.R_Categoria;
using Servicio.S_Categorias.S_SubCategorias;
using Repositorio.Repositorios.R_Categoria.R_SubCategoria;
using Repositorio.Repositorios.Token;
using Repositorio.Repositorios.R_Grupo;
using Servicio.S_Grupos;
using Repositorio.Repositorios.R_Gastos;
using Repositorio.Repositorios.R_Gastos.R_MetodosDePago;
using Servicio.DTOS.CategoriasDTO;
using Servicio.DTOS.SubCategoriasDTO;
using Servicio.Usuarios.Authentication;


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
            ValidateIssuer=true,
            ValidateAudience=true,
            ValidateLifetime =true,
            ValidateIssuerSigningKey =true,

            ValidIssuer = builder.Configuration["Jwt:Editor"],
            ValidAudience = builder.Configuration["Jwt:Audiencia"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:ClaveSecreta"])),
        };

        var accessTokenCookieName= builder.Configuration.GetValue<string>("Jwt:AccessTokenCookieName");

        o.Events = new JwtBearerEvents
        {
            OnMessageReceived = ctx =>
            {
                ctx.Request.Cookies.TryGetValue(accessTokenCookieName, out var accessToken);
                if (!string.IsNullOrEmpty(accessToken))
                    ctx.Token = accessToken;
                return Task.CompletedTask;
            }
        };
    });

// Agregar CORS con políticas personalizadas
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:4321", "https://localhost:4321", "http://localhost:5173")  // Origen de tu frontend
               .AllowCredentials()
               .AllowAnyHeader()
               .AllowAnyMethod();
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
builder.Services.AddScoped<IServicioGrupos, ServicioGrupos>();
builder.Services.AddScoped<IServicioMonedas, ServicioMonedas>();
builder.Services.AddScoped<IServicioCategoria,  ServicioCategoria>();
builder.Services.AddScoped<IServicioSubCategoria, ServicioSubCategoria>();
builder.Services.AddScoped<IServicioGasto, ServicioGasto>();

builder.Services.AddScoped<IRepositorioRefreshToken, RepositorioRefreshToken>();
builder.Services.AddScoped<IRepositorioMoneda, RepositorioMoneda>();
builder.Services.AddScoped<IRepositorioGrupo, RepositorioGrupo>();
builder.Services.AddScoped<IRepositorioSolicitud, RepositorioSolicitud>();
builder.Services.AddScoped<IRepositorioNotificacion, RepositorioNotificacion>();
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
builder.Services.AddScoped<IRepositorioCategoria, RepositorioCategoria>();
builder.Services.AddScoped<IRepositorioSubCategoria, RepositorioSubCategoria>();
builder.Services.AddScoped<IRepositorioGasto, RepositorioGasto>();
builder.Services.AddScoped<IRepositorioMetodoDePago, RepositorioMetodoDePago>();

builder.Services.AddScoped<ProveedorToken>();
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

app.UseMiddleware<TokenValidationMiddleware>();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();


app.MapControllers();

app.UseHttpsRedirection();
app.Run();
