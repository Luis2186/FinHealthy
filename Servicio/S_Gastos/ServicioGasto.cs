using Repositorio.Repositorios;
using Repositorio.Repositorios.R_Gastos.R_Categoria;
using Servicio.S_Gastos.S_Categoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.S_Gastos
{
    public class ServicioGasto : ServicioCategoria ,IServicioGasto
    {
        private readonly IRepositorioCategoria repositorioCategoria;

        public ServicioGasto(IRepositorioCategoria repoCategoria) 
            : base(repoCategoria)
        {
            repositorioCategoria = repoCategoria;
        }
    }
}
