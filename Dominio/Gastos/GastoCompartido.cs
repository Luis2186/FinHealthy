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
        public int GastoId { get; set; }
        public Gasto Gasto { get; set; }
        public int MiembroId { get; set; }
        public MiembroFamilia Miembro { get; set; }
        public double Porcentaje { get; set; }
        public double MontoAsignado { get; set; } 

        public GastoCompartido()
        {
            Gasto = new Gasto();
            Miembro = new MiembroFamilia();
        }
    }
}
