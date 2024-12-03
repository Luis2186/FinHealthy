using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Documentos
{
    public class TipoDeDocumento
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El nombre del tipo de documento es requerido")]
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
    }
}
