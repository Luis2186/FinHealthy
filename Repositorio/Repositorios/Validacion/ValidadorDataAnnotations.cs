using Dominio;
using Dominio.Abstracciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Repositorio.Repositorios.Validacion
{
    public class ValidadorDataAnnotations<T> : IValidacion<T>
    {
        public Resultado<T> Validar(T entidad)
        {
            var context = new ValidationContext(entidad);
            var resultados = new List<ValidationResult>();

            if (!Validator.TryValidateObject(entidad, context, resultados, true))
            {
                var errores = resultados
                    .Select(r => new Error(
                        code: r.MemberNames.FirstOrDefault() ?? "VALIDACION_ERROR",
                        description: r.ErrorMessage
                    ))
                    .ToList();

                return Resultado<T>.Failure(errores);
            }

            return Resultado<T>.Success(entidad);
        }
    }
}
