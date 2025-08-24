using System.ComponentModel.DataAnnotations;
using Dominio.Grupos;
using Dominio.Gastos;

namespace Dominio.Grupos
{
    public class GrupoSubCategoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GrupoId { get; set; }
        public Grupo Grupo { get; set; }

        [Required]
        public int SubCategoriaId { get; set; }
        public SubCategoria SubCategoria { get; set; }

        // Personalizaci�n por grupo
        public string? NombrePersonalizado { get; set; }
        public string? Color { get; set; }
        public bool Activo { get; set; } = true;

        // Acceso a la categor�a a trav�s de la subcategor�a
        public int CategoriaId => SubCategoria?.CategoriaId ?? 0;
        public Categoria? Categoria => SubCategoria?.Categoria;
    }
}
