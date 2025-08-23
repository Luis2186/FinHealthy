namespace Servicio.DTOS.SubCategoriasDTO
{
    public class CrearSubCategoriaDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int GrupoId { get; set; }
        public int CategoriaId { get; set; }
    }
}
