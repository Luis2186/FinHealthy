using Dominio.Gastos.IGastos;
using Dominio.Usuarios;
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
        public  Gasto Gasto { get; set; }
        public string MiembroId { get; set; }
        public  Usuario Miembro { get; set; }
        public decimal Porcentaje { get; set; }
        public decimal MontoAsignado { get; set; }
        
        public GastoCompartido(){}
        public GastoCompartido(Gasto gasto, Usuario usuario)
        {
            if (gasto == null) throw new ArgumentNullException(nameof(gasto));
            if (usuario == null) throw new ArgumentNullException(nameof(usuario));

            Gasto = gasto;
            GastoId = gasto.Id;
            Miembro = usuario;
            MiembroId = usuario.Id;
        }


    }
}
