using AutoMapper;
using Repositorio.Repositorios;
using Repositorio.Repositorios.R_Categoria;
using Servicio.S_Categorias;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.S_Gastos
{
    public class ServicioGasto : ServicioCategoria , IServicioGasto
    {
        private readonly IRepositorioCategoria repositorioCategoria;
        private readonly IMapper _mapper;

        public ServicioGasto(IRepositorioCategoria repoCategoria, IMapper mapper)
            : base(repoCategoria, mapper)
        {
            repositorioCategoria = repoCategoria;
            _mapper = mapper;
        }
    }
}
