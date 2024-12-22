using Dominio.Abstracciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public static class DataAnnotationsValidator
    {
        public static Resultado<T> Validar<T>(T objeto)
        {
            var context = new ValidationContext(objeto);
            var resultados = new List<ValidationResult>();

            if (!Validator.TryValidateObject(objeto, context, resultados, true))
            {
                var errores = resultados
                    .Select(r => new Error(
                        code: r.MemberNames.FirstOrDefault() ?? "VALIDACION_ERROR",
                        description: r.ErrorMessage
                    ))
                    .ToList();

                return Resultado<T>.Failure(errores);
            }

            return Resultado<T>.Success(objeto);
        }
    }
}
