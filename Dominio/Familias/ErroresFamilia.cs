using Dominio.Abstracciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Familias
{
    public static class ErroresFamilia
    {
        public static Func<string, Error> Error_Codigo_Vacio = (string metodo) => new Error($"{typeof(Familia).Name}.{metodo}", "El código no puede estar vacío.");
        public static Func<string, Error> Error_Codigo_Verificacion = (string metodo) => new Error($"{typeof(Familia).Name}.{metodo}", "El código es incorrecto, por favor verifiquelo.");
        public static Func<string, Error> Error_Miembro_Existente = (string metodo) => new Error($"{typeof(Familia).Name}.{metodo}", "El miembro ya existe en la familia");
    }
}
