using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dominio.Gastos
{
    public class Cuota
    {
        public int Id { get; set; }
        public int GastoId { get; set; }
        [JsonIgnore]
        public GastoEnCuotas? Gasto { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public bool Pagado { get; set; } = false;
        public Cuota(){}
    }
}
