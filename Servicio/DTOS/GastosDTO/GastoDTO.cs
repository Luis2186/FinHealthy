using Dominio.Documentos;
using Dominio.Gastos.IGastos;
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
        public string Lugar { get; set; }
        public string Etiqueta { get; set; }
        public decimal Monto { get; set; }
        public bool EsFinanciado { get; set; }
        public int CantidadDeCuotas { get; set; }
        public List<Cuota> Cuotas { get; set; }
        public bool EsCompartido { get; set; }
        public List<GastoCompartido> CompartidoCon { get; set; }
        public bool Estado { get; set; }

        public SubCategoria SubCategoria { get; set; }
        public MetodoDePago MetodoDePago { get; set; }
        public Documento? DocumentoAsociado { get; set; }
        public Moneda Moneda { get; set; }
        public int GrupoId { get; set; }
        //public GrupoDTO Grupo { get; set; }
        public string UsuarioCreadorId { get; set; }
        //public UsuarioDTO UsuarioCreador { get; set; }
    }
}
