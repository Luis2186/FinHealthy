namespace Servicio.DTOS.MetodosDePagoDTO
{
    public class MetodoDePagoDTO
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }
    }
}
