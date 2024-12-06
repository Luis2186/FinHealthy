using Dominio.Documentos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Gastos
{
    public class Gasto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "La categoria del gasto es requerida")]
        public Categoria? Categoria { get; set; }
        [Required(ErrorMessage = "El metodo de pago del gasto es requerido")]
        public MetodoDePago? MetodoDePago { get; set; }
        public Documento? DocumentoAsociado { get; set; }
        [Required(ErrorMessage = "La fecha del gasto es requerida")]
        public DateTime FechaDeGasto { get; set; }
        public string? Descripcion { get; set; }
        public string? Lugar { get; set; }
        [Required(ErrorMessage = "La moneda del gasto es requerida")]
        public Moneda? Moneda { get; set; }
        public string? Etiqueta { get; set; }
        public string? urlComprobante { get; set; }
        public bool EsFinanciado { get; set; }
        public bool EsCompartido { get; set; }
        [Required(ErrorMessage = "El monto del gasto es requerido")]
        public double Monto { get; set; }
        public List<Cuota> Cuotas { get; set; }  
        public List<GastoCompartido> CompartidoCon { get; set; }
        public bool Estado { get; set; }


        public Gasto()
        {
            this.CompartidoCon = new List<GastoCompartido>();
            this.Cuotas = new List<Cuota>();
        }

        

    }
}
