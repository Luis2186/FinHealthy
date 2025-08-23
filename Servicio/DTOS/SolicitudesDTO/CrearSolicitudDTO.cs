namespace Servicio.DTOS.SolicitudesDTO
{
    public class CrearSolicitudDTO
    {
        public required int GrupoId { get; set; }
        public required string UsuarioSolicitanteId { get; set; }
        public required string UsuarioAdministradorGrupoId { get; set; }
        public string? Mensaje { get; set; }
    }
}
