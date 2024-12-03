using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Gastos
{
    public class Moneda
    {
        [Key]
        [Required(ErrorMessage = "El codigo de la moneda es requerido")]
        // Código de la moneda, como USD, EUR
        public string? Codigo { get; set; }
        [Required(ErrorMessage = "El nombre de la moneda es requerido")]
        // Nombre de la moneda, como Dólar, Euro
        public string? Nombre { get; set; }
        [Required(ErrorMessage = "El simbolo de la moneda es requerido")]
        // Símbolo de la moneda, como $ o €
        public string? Simbolo { get; set; }
        [Required(ErrorMessage = "El tipo de cambio de la moneda es requerido")]
        // Tipo de cambio respecto a una moneda base, como USD
        public double TipoDeCambio { get; set; }
        [Required(ErrorMessage = "El pais de la moneda es requerido")]
        // País o región donde se utiliza la moneda
        public string? Pais { get; set; }

        // Método para convertir un monto de esta moneda a otra
        public double Convertir(double monto, Moneda otraMoneda)
        {
            if (otraMoneda == null)
                throw new ArgumentNullException(nameof(otraMoneda));

            return monto * (otraMoneda.TipoDeCambio / TipoDeCambio);
        }



    }
}
