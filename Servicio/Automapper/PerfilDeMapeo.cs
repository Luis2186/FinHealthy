using AutoMapper;
using Dominio.Familias;
using Dominio.Notificaciones;
using Dominio.Solicitudes;
using Dominio.Usuarios;
using Servicio.DTOS.FamiliasDTO;
using Servicio.DTOS.MiembrosFamiliaDTO;
using Servicio.DTOS.SolicitudesDTO;
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
            MapearFamilias();
            MapearMiembros();
            MapearSolicitudes();
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
        public void MapearFamilias()
        {
            CreateMap<Familia, CrearFamiliaDTO>().ReverseMap();
            CreateMap<Familia, ActualizarFamiliaDTO>().ReverseMap();
            
            CreateMap<Familia, FamiliaDTO>()
                .ForMember(dest => dest.Miembros, opt => opt.MapFrom(src => src.Miembros))
                .ReverseMap();
        }

        public void MapearMiembros()
        {
            CreateMap<MiembroFamilia, CrearMiembroFamiliaDTO>().ReverseMap();
            CreateMap<MiembroFamilia, ActualizarMiembroFamiliaDTO>().ReverseMap();
            CreateMap<MiembroFamilia, MiembroFamiliaDTO>()
                .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.Usuario))
                .ReverseMap();
        }

        public void MapearSolicitudes()
        {
            CreateMap<SolicitudUnionFamilia, EnviarSolicitudDTO>().ReverseMap();
            CreateMap<SolicitudUnionFamilia, SolicitudDTO>().ReverseMap();
        }
    }
}
