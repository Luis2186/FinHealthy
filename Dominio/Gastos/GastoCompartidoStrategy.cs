using Dominio.Gastos.IGastos;
using Dominio.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Gastos
{
    public class GastoCompartidoStrategy : IGastoStrategy
    {
        public Resultado<Gasto> CalcularGasto(Gasto gasto, List<Usuario> usuarios = null)
        {
            throw new NotImplementedException();
        }
    }
}
