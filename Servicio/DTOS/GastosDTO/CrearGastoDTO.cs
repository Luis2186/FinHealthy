using Dominio.Documentos;
using Dominio.Gastos;
using Servicio.DTOS.UsuariosDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.DTOS.GastosDTO
{
    public class CrearGastoDTO
    {
        [Required(ErrorMessage = "La fecha en que fue efectuado el gasto es obligatoria")]
        public DateTime FechaDeGasto { get; set; }
        [Required(ErrorMessage = "La descripción del gasto es obligatoria")]
        public string Descripcion { get; set; }
        public string? Lugar { get; set; }
        public string? Etiqueta { get; set; }
        [Required(ErrorMessage = "El monto del gasto es obligatorio")]
        public decimal Monto { get; set; }
        public bool EsFinanciado { get; set; }
        public int CantidadDeCuotas { get; set; }
        public bool EsCompartido { get; set; }

        public List<string> UsuariosCompartidosIds { get; set; }
        [Required(ErrorMessage = "La categoria es obligatoria")]
        public int SubCategoriaId { get; set; }
        [Required(ErrorMessage = "El metodo de pago es obigatorio")]
        public int MetodoDePagoId { get; set; }
        [Required(ErrorMessage = "La moneda es obligatoria")]
        public string MonedaId { get; set; }
        public int? DocumentoAsociadoId { get; set; }
    }
}
