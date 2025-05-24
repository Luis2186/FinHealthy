using Dominio.Grupos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dominio.Gastos
{
    public class SubCategoria
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        
        public int CategoriaId { get; set; }
        [Required]
        public Categoria Categoria { get; set; }
        
        public int GrupoId { get; set; }
        [JsonIgnore]
        public Grupo GrupoGasto { get; set; }

        public SubCategoria()
        {
        }
    }
}
