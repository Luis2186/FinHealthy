using System;
using System.Collections.Generic;
using Dominio.Gastos;

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
}

public class GastoFijoDTO : GastoDTO
{
    public bool EsFijo { get; set; }
    // Ejemplo de propiedad específica: periodicidad (mensual, anual, etc.)
    public string? Periodicidad { get; set; }
    // Fecha de inicio del gasto fijo
    public DateTime? FechaInicio { get; set; }
    // Fecha de fin del gasto fijo (si aplica)
    public DateTime? FechaFin { get; set; }
}

public class GastoCompartidoDTO : GastoDTO
{
    public List<GastoCompartidoDetalleDTO> CompartidoCon { get; set; } = new();
}

public class GastoCompartidoDetalleDTO
{
    public string UsuarioId { get; set; }
    public string NombreUsuario { get; set; }
    public decimal MontoAsignado { get; set; }
    public decimal Porcentaje { get; set; }
}

public class GastoCuotaDTO : GastoDTO
{
    public int CantidadDeCuotas { get; set; }
    public List<Cuota> Cuotas { get; set; } = new();
}

public class GastosSegmentadosDTO
{
    public List<GastoFijoDTO> GastosFijos { get; set; } = new();
    public List<GastoDTO> GastosMensuales { get; set; } = new();
    public List<GastoCompartidoDTO> GastosCompartidos { get; set; } = new();
    public List<GastoCuotaDTO> GastosEnCuotas { get; set; } = new();
}
