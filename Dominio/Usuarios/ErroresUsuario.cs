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
        public static readonly Error NombreDeUsuarioVacio = new("Usuario.Registrar", "El usuario no existe");
        public static readonly Error ApellidoVacio = new("Usuario.Registrar", "El usuario no existe");
        public static readonly Error FechaInvalida = new("Usuario.Registrar", "La fecha que intenta ingresar es invalida");
        public static readonly Error Telefono = new("Usuario.Registrar", "El usuario no existe");
        public static readonly Error UsuarioInexistente = new("Usuario.Buscar", "El usuario no existe");
    }
}

