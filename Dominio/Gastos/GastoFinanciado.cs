using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Gastos
{
    public class GastoFinanciado
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "La cantidad de cuotas del gasto financiado es requerida")]
        public int CantidadDeCuotas { get; set; }
        public int CuotasPagadas { get; set; }
        public int CuotasRestantes { get; set; }
        [Required(ErrorMessage = "La frencuencia del gasto es requerida")]
        public string? Frecuencia { get; set; } //Mensual, Anual, Trimestral, Semestral, etc
    }
}
