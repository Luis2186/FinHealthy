using Dominio.Documentos;
using Dominio.Gastos;
using Dominio.Grupos;
using Dominio.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.DTOS.GastosDTO
{
    public interface IFormarGastoDTO
    {
        int GrupoId { get; set; }
        string UsuarioCreadorId { get; set; }
        int SubCategoriaId { get; set; }
        int MetodoDePagoId { get; set; }
        string MonedaId { get; set; }
        List<string> UsuariosCompartidosIds { get; set; }
        DateTime FechaDeGasto { get; set; }
        string Descripcion { get; set; }
        string Lugar { get; set; }
        string Etiqueta { get; set; }
        bool EsFinanciado { get; set; }
        int CantidadDeCuotas { get; set; }
        bool EsCompartido { get; set; }
        decimal Monto { get; set; }

    }
}
