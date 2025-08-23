namespace Servicio.DTOS.NotificacionesDTO
{
    public class NotificacionDTO
    {
        public int Id { get; set; }
        public required string UsuarioEmisorId { get; set; }
        public required string UsuarioReceptorId { get; set; }
        public required string Titulo { get; set; }
        public required string Mensaje { get; set; }
        public bool Leida { get; set; }
        public DateTime Fecha { get; set; }
    }
}
