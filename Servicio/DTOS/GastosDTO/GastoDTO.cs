using Dominio.Documentos;
using Dominio.Gastos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servicio.DTOS.GruposDTO;
using Servicio.DTOS.UsuariosDTO;

namespace Servicio.DTOS.GastosDTO
{
    public class GastoDTO
    {
        public int Id { get; set; }
        public DateTime FechaDeGasto { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public string Lugar { get; set; }
        public string Etiqueta { get; set; }
        public bool Estado { get; set; }
        public int GrupoId { get; set; }
        public string UsuarioCreadorId { get; set; }
        // Propiedades para lógica original
        public bool EsFinanciado { get; set; }
        public int CantidadDeCuotas { get; set; }
        public List<Cuota> Cuotas { get; set; } = new();
        public bool EsCompartido { get; set; }
        public List<GastoCompartido> CompartidoCon { get; set; } = new();
    }
}
