using Dominio.Documentos;
using Dominio.Gastos;
using Dominio.Gastos.IGastos;
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
        int GrupoId { get; }
        string UsuarioCreadorId { get; }
        int SubCategoriaId { get; }
        int MetodoDePagoId { get; }
        string MonedaId { get; }
        List<string> UsuariosCompartidosIds { get; }
        DateTime FechaDeGasto { get;}
        string Descripcion { get;}
        string Lugar { get;}
        string Etiqueta { get; }
        bool EsFinanciado { get; }
        int CantidadDeCuotas { get; }
        bool EsCompartido { get; }
        decimal Monto { get; }

    }
}
