using Dominio.Familias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Gastos
{
    public class GastoCompartido
    {
        public int Id { get; set; }
        public List<MiembroFamilia> Miembros { get; set; }
        public bool EnPartesIguales { get; set; }
        public Dictionary<MiembroFamilia,double> ImportePorMiembro { get; set; }
    }
}
