using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.Validacion
{
    public interface IValidacion<T>
    {
        public Resultado<T> Validar(T entidad);
    }
}
