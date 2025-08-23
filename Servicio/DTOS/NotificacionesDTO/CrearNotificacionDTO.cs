namespace Servicio.DTOS.NotificacionesDTO
{
    public class CrearNotificacionDTO
    {
        public required string UsuarioEmisorId { get; set; }
        public required string UsuarioReceptorId { get; set; }
        public required string Titulo { get; set; }
        public required string Mensaje { get; set; }
    }
}
