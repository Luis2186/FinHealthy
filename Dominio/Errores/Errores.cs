using Dominio.Abstracciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Errores
{
    public static class Errores
    {
        //Errores Excepcion
        public static Func<string,string, Error> ErrorDeExcepcion = (string metodo,string ex) => new Error($"ERROR_EXCEPTION_{metodo}", $"Ah ocurrido un error inesperado: {ex}");


        //Errores Crud
        public static Func<string, Error> ErrorDeActualizacion = (string entidad) => new Error($"ERROR_UPDATE_{entidad}", $"Ah ocurrido un error al intentar actualizar el/la {entidad}");
        public static Func<string,Error> ErrorDeCreacion = (string entidad) => new Error($"ERROR_CREATE_{entidad}",$"Ah ocurrido un error al intentar crear el/la {entidad} ");
        public static Func<string, Error> ErrorBuscarPorId = (string entidad) => new Error($"ERROR_FINDBYID_{entidad}", $"No se encontró el/la {entidad} solicitado/a");
        public static Func<string, Error> ErrorDeEliminacion = (string entidad) => new Error($"ERROR_REMOVE_{entidad}", $"Ah ocurrido un error al intentar eliminar el/la {entidad} ");
        public static Func<string, Error> ErrorBuscarTodos = (string entidad) => new Error($"ERROR_REMOVE_{entidad}", $"No se encontraron {entidad}");

        





    }
}
