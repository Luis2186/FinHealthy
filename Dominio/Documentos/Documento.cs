using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Documentos
{
    public class Documento
    {
        public int Id { get; set; }
        public string? Numero { get; set; }
        [Required(ErrorMessage = "El tipo de documento es requerido")]
        public TipoDeDocumento? TipoDeDocumento { get; set; }
        [Required(ErrorMessage = "La entiedad emisora del documento requerida")]
        public string? EntidadEmisora { get; set; }
        [Required(ErrorMessage = "La fecha de emision del documento es requerido")]
        public DateTime FechaDeEmision { get; set; }
        [Required(ErrorMessage = "La fecha de vencimiento del documento es requerido")]
        public DateTime FechaDeVencimiento { get; set; }
        public string? urlArchivoAdjunto { get; set; }
        public string? Descripcion { get; set; }
        public string? Etiqueta { get; set; }
        public string? Observaciones { get; set; }
        public bool Estado { get; set; }
    }
}
