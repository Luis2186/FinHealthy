namespace Servicio.DTOS.NotificacionesDTO
{
    public class ActualizarNotificacionDTO
    {
        public required string Titulo { get; set; }
        public required string Mensaje { get; set; }
        public bool Leida { get; set; }
    }
}
