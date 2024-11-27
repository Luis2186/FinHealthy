using AutoMapper;
using Dominio.Notificaciones;
using Dominio.Usuarios;
using Servicio.Notificaciones.NotificacionesDTO;
using Servicio.Usuarios.UsuariosDTO;
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
          .ForMember(destino => destino.NombreDeUsuario, opt => opt.MapFrom(origen => origen.UserName));

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

    }
}
