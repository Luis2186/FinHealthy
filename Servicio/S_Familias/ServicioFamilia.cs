using Repositorio.Repositorios.R_Familias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.S_Familias
{
    public class ServicioFamilia : IServicioFamilia
    {
        public readonly IRepositorioGrupoFamilia _repoGrupoFamilia;
        public readonly IRepositorioMiembroFamilia _repoMiembroFamilia;

        public ServicioFamilia(IRepositorioGrupoFamilia repoGrupoFamilia,
                               IRepositorioMiembroFamilia repoMiembroFamilia  )
        {
            _repoGrupoFamilia = repoGrupoFamilia;
            _repoMiembroFamilia = repoMiembroFamilia;
        }






    }
}
