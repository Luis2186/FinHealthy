using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Gastos
{
    public class MetodoDePago
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre del metodo de pago es requerido")]
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
    }
}
