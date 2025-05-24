using AutoMapper;
using Dominio.Gastos;
using Dominio.Grupos;
using Dominio.Notificaciones;
using Dominio.Solicitudes;
using Dominio.Usuarios;
using Microsoft.AspNetCore.SignalR;
using Servicio.DTOS.CategoriasDTO;
using Servicio.DTOS.GastosDTO;
using Servicio.DTOS.GruposDTO;
using Servicio.DTOS.SolicitudesDTO;
using Servicio.DTOS.SubCategoriasDTO;
using Servicio.DTOS.UsuariosDTO;
using Servicio.Notificaciones.NotificacionesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.Automapper
{
    public class PerfilDeMapeo : Profile
    {
        public PerfilDeMapeo() {

            MapearUsuarios();
            MapearNotificaciones();
            MapearGrupos();
            //MapearMiembros();
            MapearSolicitudes();
            MapearCategorias();
            MapearSubCategorias();
            MapearGastos();
        }

        private int CalcularEdad(DateTime fechaNacimiento)
        {
            var today = DateTime.Today;
            var edad = today.Year - fechaNacimiento.Year;

            // Ajustar la edad si aún no ha pasado el cumpleaños este año
            if (fechaNacimiento.Date > today.AddYears(-edad)) edad--;

            return edad;
        }

        public void MapearUsuarios()
        {
            CreateMap<Usuario, UsuarioPDFDTO>()
            .ForMember(destino => destino.Telefono, opt => opt.MapFrom(origen => origen.PhoneNumber)).ReverseMap();

            CreateMap<UsuarioDTO, UsuarioPDFDTO>().ReverseMap();

            CreateMap<Usuario, UsuarioDTO>()
              .ForMember(destino => destino.Telefono, opt => opt.MapFrom(origen => origen.PhoneNumber))
              .ForMember(destino => destino.NombreDeUsuario, opt => opt.MapFrom(origen => origen.UserName))
              .ForMember(destino => destino.GrupoDeGastos, opt => opt.MapFrom(origen => origen.GrupoDeGastos))  // Mapeo del grupo
              .ReverseMap();

            CreateMap<CrearUsuarioDTO, Usuario>()
                .ForMember(destino => destino.PhoneNumber, opt => opt.MapFrom(origen => origen.Telefono))
                .ForMember(destino => destino.UserName, opt => opt.MapFrom(origen => origen.NombreDeUsuario))
                .ForMember(dest => dest.Edad, opt => opt.MapFrom(src => CalcularEdad(src.FechaDeNacimiento)))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => true));

            CreateMap<Usuario, CrearUsuarioDTO>()
               .ForMember(destino => destino.Telefono, opt => opt.MapFrom(origen => origen.PhoneNumber))
               .ForMember(destino => destino.NombreDeUsuario, opt => opt.MapFrom(origen => origen.UserName));

            CreateMap<ActualizarUsuarioDTO, Usuario>()
                .ForMember(destino => destino.PhoneNumber, opt => opt.MapFrom(origen => origen.Telefono))
                .ForMember(destino => destino.UserName, opt => opt.MapFrom(origen => origen.NombreDeUsuario))
                .ForMember(dest => dest.Edad, opt => opt.MapFrom(src => CalcularEdad(src.FechaDeNacimiento)));

            CreateMap<Usuario, ActualizarUsuarioDTO>()
               .ForMember(destino => destino.Telefono, opt => opt.MapFrom(origen => origen.PhoneNumber))
               .ForMember(destino => destino.NombreDeUsuario, opt => opt.MapFrom(origen => origen.UserName));
        }

        public void MapearNotificaciones()
        {
            CreateMap<Notificacion, NotificacionCreacionDTO>();
            CreateMap<NotificacionCreacionDTO, Notificacion>();

            CreateMap<Notificacion, NotificacionDTO>();
            CreateMap<NotificacionDTO, Notificacion>();
        }
        public void MapearGrupos()
        {
            CreateMap<Grupo, CrearGrupoDTO>().ReverseMap();

            CreateMap<Grupo, ActualizarGrupoDTO>().ReverseMap();
            
            CreateMap<Grupo, GrupoDTO>()
                 //.ForMember(dest => dest.Miembros, opt => opt.MapFrom(src => src.MiembrosGrupoGasto))
                 .ForMember(dest => dest.Miembros, opt => opt.MapFrom(src => MapearMiembrosGrupoGasto(src.MiembrosGrupoGasto))) // Evita mapear de nuevo la propiedad GrupoDeGastos para prevenir ciclos
                .ReverseMap();
        }

        public void MapearSolicitudes()
        {
            CreateMap<SolicitudUnionGrupo, EnviarSolicitudDTO>().ReverseMap();
            CreateMap<SolicitudUnionGrupo, SolicitudDTO>().ReverseMap();
        }

        public void MapearCategorias()
        {
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            //    ForMember(cat => cat.SubCategorias, opt => opt.MapFrom(src => src.SubCategorias)).ReverseMap();
        }
        public void MapearSubCategorias()
        {
            CreateMap<SubCategoriaDTO, ActualizarCategoriaDTO>();

            CreateMap<SubCategoria, SubCategoriaDTO>()
                .ForMember(cat => cat.GrupoId, opt => opt.MapFrom(src => src.GrupoGasto.Id))
                .ForMember(cat => cat.CategoriaId, opt => opt.MapFrom(src => src.Categoria.Id))
                .ReverseMap();

            CreateMap<SubCategoria, ActualizarCategoriaDTO>()
            .ForMember(cat => cat.GrupoId, opt => opt.MapFrom(src => src.GrupoGasto.Id))
            .ForMember(cat => cat.CategoriaId, opt => opt.MapFrom(src => src.Categoria.Id))
            .ReverseMap();
        }

        public void MapearGastos()
        {
            CreateMap<Gasto, GastoDTO>()
                .ForMember(gasto => gasto.SubCategoria, opt => opt.MapFrom(src => src.SubCategoria))
                .ForMember(gasto => gasto.Moneda, opt => opt.MapFrom(src => src.Moneda))
                .ForMember(gasto => gasto.MetodoDePago, opt => opt.MapFrom(src => src.MetodoDePago))
                .ForMember(gasto => gasto.Cuotas, opt => opt.MapFrom(src => src.Cuotas))
                .ForMember(gasto => gasto.CompartidoCon, opt => opt.MapFrom(src => src.CompartidoCon))
                .ReverseMap();
        }




        public List<UsuarioDTO> MapearMiembrosGrupoGasto(IEnumerable<Usuario> miembrosGrupoGasto)
        {
            if (miembrosGrupoGasto == null)
            {
                return new List<UsuarioDTO>();
            }

            return miembrosGrupoGasto.Select(m => new UsuarioDTO
            {
                Id = m.Id,
                NombreDeUsuario = m.UserName,
                Nombre = m.Nombre,
                Apellido = m.Apellido,
                Edad = m.Edad,
                Email = m.Email,
                Telefono = m.PhoneNumber,
                FechaDeNacimiento = m.FechaDeNacimiento,
                FechaDeRegistro = m.FechaDeRegistro,
                Roles = m.Roles,
                Activo = m.Activo
            }).ToList();
        }

    }
}
