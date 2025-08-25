using System;
using System.Collections.Generic;
using Servicio.DTOS.GastosDTO;
using Dominio.Gastos;

public class GastosSegmentadosDTO
{
    public List<GastoDTO> GastosFijos { get; set; } = new();
    public List<GastoCompartidoDTO> GastosCompartidos { get; set; } = new();
    public List<GastoCuotaDTO> GastosEnCuotas { get; set; } = new();
}

public class GastoCompartidoDTO
{
    public int Id { get; set; }
    public DateTime FechaDeGasto { get; set; }
    public string Descripcion { get; set; }
    public decimal Monto { get; set; }
    public List<GastoCompartidoDetalleDTO> CompartidoCon { get; set; } = new();
}

public class GastoCompartidoDetalleDTO
{
    public string UsuarioId { get; set; }
    public string NombreUsuario { get; set; }
    public decimal MontoAsignado { get; set; }
    public decimal Porcentaje { get; set; }
}

public class GastoCuotaDTO
{
    public int Id { get; set; }
    public DateTime FechaDeGasto { get; set; }
    public string Descripcion { get; set; }
    public decimal Monto { get; set; }
    public int CantidadDeCuotas { get; set; }
    public List<Cuota> Cuotas { get; set; } = new();
}
