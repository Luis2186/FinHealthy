﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Gastos
{
    public class Cuota
    {
        public int Id { get; set; }
        public int GastoId { get; set; }
        public Gasto Gasto { get; set; }
        public double Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public bool Pagado { get; set; } = false;

        public Cuota()
        {
            Gasto = new Gasto();
        }
    }
}
