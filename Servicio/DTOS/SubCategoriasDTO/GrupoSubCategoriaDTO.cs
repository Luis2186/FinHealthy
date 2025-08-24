namespace Servicio.DTOS.SubCategoriasDTO
{
    public class GrupoSubCategoriaDTO
    {
        public int Id { get; set; }
        public int SubCategoriaId { get; set; }
        public string? NombrePersonalizado { get; set; }
        public string? Color { get; set; }
        public bool Activo { get; set; }
    }

    public class CrearGrupoSubCategoriaDTO
    {
        public int SubCategoriaId { get; set; }
        public string? NombrePersonalizado { get; set; }
        public string? Color { get; set; }
    }

    public class ActualizarGrupoSubCategoriaDTO
    {
        public string? NombrePersonalizado { get; set; }
        public string? Color { get; set; }
        public bool Activo { get; set; }
    }
}
