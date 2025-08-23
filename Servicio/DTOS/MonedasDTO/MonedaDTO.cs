namespace Servicio.DTOS.MonedasDTO
{
    public class MonedaDTO
    {
        public int Id { get; set; }
        public required string Codigo { get; set; }
        public required string Nombre { get; set; }
        public required string Simbolo { get; set; }
    }
}
