using Dominio.Abstracciones;
using Dominio.Grupos;
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
        public static Func<string, Error> EmailExistente = (string metodo) => new Error($"{typeof(Usuario).Name}.{metodo}", "Ya existe un usuario con ese correo.");
        public static Func<string, Error> NombreDeUsuarioExistente = (string metodo) => new Error($"{typeof(Usuario).Name}.{metodo}", "Ya existe un usuario registrado con ese nombre");
        public static Func<string, Error> ConfirmacionContraseña = (string metodo) => new Error($"{typeof(Usuario).Name}.{metodo}", "Las contraseñas deben coincidir, por favor reviselas.");
        public static Func<string, Error> CredencialesInvalidas = (string metodo) => new Error($"{typeof(Usuario).Name}.{metodo}", "El usuario y/o la contraseña son incorrectos.");

        public static Func<string, Error> EmailInexistente = (string metodo) => new Error($"{typeof(Usuario).Name}.{metodo}", "No se encontró un usuario con el email proporcionado.");
        public static Func<string, Error> IdInexistente = (string metodo) => new Error($"{typeof(Usuario).Name}.{metodo}", "No se encontró un usuario con el id proporcionado.");


        public static Func<string, Error> UsuarioSinClaims = (string metodo) => new Error($"{typeof(Usuario).Name}.{metodo}", "El usuario no tiene ningún claim asignado.");
        public static Func<string, Error> UsuarioSinRoles = (string metodo) => new Error($"{typeof(Usuario).Name}.{metodo}", "No existen roles registrados en el sistema.");
        public static Func<string, Error> RolNoEncontrado = (string metodo) => new Error($"{typeof(Usuario).Name}.{metodo}", "No se encontró el rol con el id o nombre proporcionado.");

        public static Func<string, Error> NombreDeUsuarioVacio = (string metodo) => new Error($"{typeof(Usuario).Name}.{metodo}", "El usuario no existe");
        public static Func<string, Error> ApellidoVacio = (string metodo) => new Error($"{typeof(Usuario).Name}.{metodo}", "El usuario no existe");
        public static Func<string, Error> FechaInvalida = (string metodo) => new Error($"{typeof(Usuario).Name}.{metodo}", "La fecha que intenta ingresar es invalida");
        public static Func<string, Error> Telefono = (string metodo) => new Error($"{typeof(Usuario).Name}.{metodo}", "El usuario no existe");

        public static Func<string, Error> Datos_Invalidos = (string metodo) => new Error($"{typeof(Usuario).Name}.{metodo}", "Los datos proporcionados no se encuentran o son invalidos, por favor verifiquelos");
        public static Func<string, Error> Login = (string metodo) => new Error($"{typeof(Usuario).Name}.{metodo}", "El usuario y/o la contraseña son incorrectos.");
        public static Func<string, Error> UsuarioInexistente = (string metodo) => new Error($"{typeof(Usuario).Name}.{metodo}", "El usuario no existe en los registros");
        public static Func<string, Error> UsuarioInhabilitado = (string metodo) => new Error($"{typeof(Usuario).Name}.{metodo}", "El usuario se encuentra inhabilitado");

    }
}

