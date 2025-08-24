using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.DTOS.GastosDTO
{
    public class ActualizarGastoDTO : CrearGastoDTO, IFormarGastoDTO
    {
        [Required]
        public int Id { get; set; }
    }
}
