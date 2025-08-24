using Dominio.Abstracciones;
using Dominio.Errores;
using Dominio.Gastos;
using Dominio.Usuarios;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Grupos
{
    public class Grupo
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre del grupo es un campo requerido, por favor ingreselo")]
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaDeCreacion { get; set; }
        [Required(ErrorMessage = "El administrador es un campo requerido, por favor ingreselo")]
        public Usuario? UsuarioAdministrador { get; set; }
        public string? UsuarioAdministradorId { get; set; }
        [Required(ErrorMessage = "El codigo de acceso es un campo requerido, por favor ingreselo")]
        public string? CodigoAccesoHash { get; private set; }
        public List<Usuario> MiembrosGrupoGasto { get; set; } = new List<Usuario>();
        public bool Activo { get; set; }
        public List<GrupoSubCategoria> GrupoSubCategorias { get; set; } = new();

        public Grupo()
        {
           
        }
        public Grupo(Usuario usuarioAdministrador, string nombre, string descripcion, string codigo)
        {
            this.UsuarioAdministrador = usuarioAdministrador;
            this.UsuarioAdministradorId = usuarioAdministrador.Id;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.FechaDeCreacion = DateTime.Now;
            this.MiembrosGrupoGasto = new List<Usuario>() { usuarioAdministrador };
            this.Activo = true;
            EstablecerCodigo(codigo);
        }

        // Método para establecer el código
        public Resultado<bool> EstablecerCodigo(string codigo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codigo))
                    return Resultado<bool>.Failure(ErroresGrupo.Codigo_Vacio("EstablecerCodigo"));

                CodigoAccesoHash = BCrypt.Net.BCrypt.HashPassword(codigo);

                return Resultado<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion("EstablecerCodigo",ex.Message));
            }
        }

        // Método para verificar el código
        public Resultado<bool> VerificarCodigo(string codigo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codigo))
                    return Resultado<bool>.Failure(ErroresGrupo.Codigo_Vacio("VerificarCodigo"));

                var verificado = BCrypt.Net.BCrypt.Verify(codigo, CodigoAccesoHash);

                if (!verificado) return Resultado<bool>.Failure(ErroresGrupo.Codigo_Verificacion("VerificarCodigo"));
                
                return Resultado<bool>.Success(verificado);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion("VerificarCodigo", ex.Message));
            }
        }

        public Resultado<bool> AgregarMiembro(Usuario miembro)
        {
            if (!MiembrosGrupoGasto.Contains(miembro))
            {
                MiembrosGrupoGasto.Add(miembro);
            }
            else
            {
                return Resultado<bool>.Failure(ErroresGrupo.Miembro_Existente("AgregarMiembroAGrupo"));
            }
            return Resultado<bool>.Success(true);   
        }

        public Resultado<bool> AgregarUsuarioAdministrador(Usuario admin)
        {
            this.UsuarioAdministrador = admin;
            this.UsuarioAdministradorId = admin.Id;

            return Resultado<bool>.Success(UsuarioAdministrador != null && UsuarioAdministradorId != "");
        }

        public Resultado<bool> AgregarGrupoSubCategoria(GrupoSubCategoria grupoSubCategoria)
        {
            if (grupoSubCategoria == null || grupoSubCategoria.SubCategoria == null || string.IsNullOrWhiteSpace(grupoSubCategoria.SubCategoria.Nombre))
                return Resultado<bool>.Failure(new Error("Grupo.AgregarGrupoSubCategoria", "La subcategoría es nula o su nombre es inválido."));

            bool existe = GrupoSubCategorias.Any(gsc => gsc.SubCategoriaId == grupoSubCategoria.SubCategoriaId);
            if (existe)
                return Resultado<bool>.Failure(new Error("Grupo.AgregarGrupoSubCategoria", $"Ya existe una subcategoría con el id '{grupoSubCategoria.SubCategoriaId}' en el grupo."));

            GrupoSubCategorias.Add(grupoSubCategoria);
            return Resultado<bool>.Success(true);
        }

        public Resultado<Gasto> ValidarConsistenciaSubcategoria(Gasto gasto)
        {
            if (gasto == null || gasto.SubCategoria == null)
                return Resultado<Gasto>.Failure(new Error("Grupo.ValidarConsistenciaSubcategoria", "El gasto o la subcategoría es nula."));

            bool existe = GrupoSubCategorias.Any(gsc => gsc.SubCategoriaId == gasto.SubCategoria.Id && gsc.Activo);
            if (!existe)
                return Resultado<Gasto>.Failure(new Error("Grupo.ValidarConsistenciaSubcategoria", "La subcategoría seleccionada no está habilitada para este grupo."));

            return Resultado<Gasto>.Success(gasto);
        }
    }
}
