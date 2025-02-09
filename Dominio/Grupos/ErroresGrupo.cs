using Dominio.Abstracciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Grupos
{
    public static class ErroresGrupo
    {
        public static Func<string, Error> Codigo_Vacio = (string metodo) => new Error($"{typeof(Grupo).Name}.{metodo}", "El código no puede estar vacío.");
        public static Func<string, Error> Codigo_Verificacion = (string metodo) => new Error($"{typeof(Grupo).Name}.{metodo}", "El código es incorrecto, por favor verifiquelo.");
        public static Func<string, Error> Miembro_Existente = (string metodo) => new Error($"{typeof(Grupo).Name}.{metodo}", "El usuario ya es integrante del grupo");
        public static Func<string, Error> Datos_Invalidos = (string metodo) => new Error($"{typeof(Grupo).Name}.{metodo}", "Los datos proporcionados no se encuentran o son invalidos, por favor verifiquelos");

    }
}
