using Dominio.Abstracciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Usuarios
{
    public static class ErroresUsuario
    {
        public static readonly Error EmailExistente = new("Usuario.Registrar", "Ya existe un usuario con ese correo");
        public static readonly Error NombreDeUsuarioExistente = new("Usuario.Registrar", "Ya existe un usuario registrado con ese nombre");
        public static readonly Error ConfirmacionContraseña = new("Usuario.Registrar", "Las contraseñas deben coincidir, por favor reviselas.");
        public static readonly Error CredencialesInvalidas = new("Usuario.Login", "El usuario y/o la contraseña son incorrectos.");

        public static readonly Error EmailInexistente = new("Usuario.Buscar", "No se encontró un usuario con el email proporcionado.");
        public static readonly Error IdInexistente = new("Usuario.Buscar", "No se encontró un usuario con el id proporcionado.");

        public static readonly Error UsuarioSinClaims = new("Usuario.SinClaims", "El usuario no tiene ningún claim asignado.");
        public static readonly Error UsuarioSinRoles = new("Usuario.SinRoles", "No existen roles registrados en el sistema.");
        public static readonly Error RolNoEncontrado = new("Rol.Buscar", "No se encontró el rol con el id o nombre proporcionado.");


        public static readonly Error NombreDeUsuarioVacio = new("Usuario.Registrar", "El usuario no existe");
        public static readonly Error ApellidoVacio = new("Usuario.Registrar", "El usuario no existe");
        public static readonly Error FechaInvalida = new("Usuario.Registrar", "La fecha que intenta ingresar es invalida");
        public static readonly Error Telefono = new("Usuario.Registrar", "El usuario no existe");
    }
}

