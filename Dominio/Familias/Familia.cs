using Dominio.Abstracciones;
using Dominio.Errores;
using Dominio.Usuarios;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Familias
{
    public class Familia
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El apellido de la familia es un campo requerido, por favor ingreselo")]
        public string? Apellido { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaDeCreacion { get; set; }
        [Required(ErrorMessage = "El administrador es un campo requerido, por favor ingreselo")]
        public Usuario UsuarioAdministrador { get; set; }
        public string UsuarioAdministradorId { get; set; }
        [Required(ErrorMessage = "El codigo de acceso es un campo requerido, por favor ingreselo")]
        public string CodigoAccesoHash { get; private set; }
        public List<MiembroFamilia> Miembros { get; set; }
        public bool Activo { get; set; }

        public Familia()
        {
            
        }
        public Familia(Usuario usuarioAdministrador, string apellido, string descripcion, string codigo)
        {
            this.UsuarioAdministrador = usuarioAdministrador;
            this.UsuarioAdministradorId = usuarioAdministrador.Id;
            this.Apellido = apellido;
            this.Descripcion = descripcion;
            this.FechaDeCreacion = DateTime.Now;
            this.Miembros = new List<MiembroFamilia>() { new MiembroFamilia(usuarioAdministrador,this) };
            this.Activo = true;
        }

        // Método para establecer el código
        public Resultado<bool> EstablecerCodigo(string codigo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codigo))
                    return Resultado<bool>.Failure(ErroresFamilia.Error_Codigo_Vacio("EstablecerCodigo"));

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
                    return Resultado<bool>.Failure(ErroresFamilia.Error_Codigo_Vacio("VerificarCodigo"));

                var verificado = BCrypt.Net.BCrypt.Verify(codigo, CodigoAccesoHash);

                if (!verificado) return Resultado<bool>.Failure(ErroresFamilia.Error_Codigo_Verificacion("VerificarCodigo"));
                
                return Resultado<bool>.Success(verificado);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion("VerificarCodigo", ex.Message));
            }
        }
    }
}
