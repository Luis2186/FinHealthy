using Dominio.Grupos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Gastos
{
    public class SubCategoria
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int CategoriaId { get; set; }
        public int GrupoId { get; set; }

        public Categoria Categoria { get; set; }
        public Grupo GrupoGasto { get; set; }

        public SubCategoria()
        {
            this.Categoria = new Categoria();
            this.GrupoGasto = new Grupo();
        }
    }
}
