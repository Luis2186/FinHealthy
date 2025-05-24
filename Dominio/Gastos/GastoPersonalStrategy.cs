using Dominio.Abstracciones;
using Dominio.Gastos.IGastos;
using Dominio.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Gastos
{
    public class GastoPersonalStrategy : IGastoStrategy
    {
        public Resultado<Gasto> CalcularGasto(Gasto gasto, List<Usuario> usuarios = null)
        {
            if (gasto.Monto <= 0) return Resultado < Gasto >.Failure(new Error("GastoPersonalStrategy.CalcularGasto", "El gasto debe ser mayor a 0 "));

            return gasto;
        }
    }
}
