namespace Servicio.DTOS.TipoDeDocumentoDTO
{
    public class TipoDeDocumentoDTO
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }
    }
}
