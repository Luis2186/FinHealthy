using Dominio.Abstracciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Usuarios
{
    public static class ErroresToken
    {
        public static Func<string, Error> Invalido = (string metodo) => new Error($"{typeof(RefreshToken).Name}.{metodo}", "Refresh token inválido o expirado.");
    }
}
