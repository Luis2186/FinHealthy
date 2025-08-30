using Servicio.DTOS.GastosDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public abstract class CrearGastoBaseDTO : IFormarGastoDTO
{
    [Required] public int GrupoId { get; set; }
    [Required] public string UsuarioCreadorId { get; set; }
    [Required] public int SubCategoriaId { get; set; }
    [Required] public int MetodoDePagoId { get; set; }
    [Required] public string MonedaId { get; set; }
    [Required] public DateTime FechaDeGasto { get; set; }
    [Required] public string Descripcion { get; set; }
    [Required] public string Lugar { get; set; }
    [Required] public string Etiqueta { get; set; }
    [Required] public decimal Monto { get; set; }
    public List<string> UsuariosCompartidosIds { get; set; } = new();
    public bool EsFinanciado { get; set; } = false;
    public int CantidadDeCuotas { get; set; } = 0;
    public bool EsCompartido { get; set; } = false;
}

public class CrearGastoFijoDTO : CrearGastoBaseDTO
{
    [Required] public string Periodicidad { get; set; } = string.Empty;
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
}

public class CrearGastoMensualDTO : CrearGastoBaseDTO
{
    // No requiere campos adicionales
}

public class CrearGastoCompartidoDTO : CrearGastoBaseDTO
{
    [Required] public new List<string> UsuariosCompartidosIds { get; set; } = new();
    public List<decimal>? PorcentajesCompartidos { get; set; } // Opcional: si se quiere asignar porcentajes
    public CrearGastoCompartidoDTO()
    {
        EsCompartido = true;
    }

    /// <summary>
    /// Devuelve la lista de IDs de participantes del gasto, incluyendo al creador.
    /// </summary>
    public List<string> ObtenerParticipantesIds()
    {
        var ids = new List<string>(UsuariosCompartidosIds);
        if (!ids.Contains(UsuarioCreadorId))
            ids.Insert(0, UsuarioCreadorId);
        return ids;
    }
}

public class CrearGastoEnCuotasDTO : CrearGastoBaseDTO
{
    [Required] public new int CantidadDeCuotas { get; set; }
    public CrearGastoEnCuotasDTO()
    {
        EsFinanciado = true;
    }
}
