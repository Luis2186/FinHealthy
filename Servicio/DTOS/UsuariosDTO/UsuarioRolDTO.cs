using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.DTOS.UsuariosDTO
{
    public class UsuarioRolDTO : IValidatableObject
    {
        [Required(ErrorMessage = "El Id del usuario es obligatorio.")]
        [StringLength(100, ErrorMessage = "El ID del usuario no puede tener más de 100 caracteres.")]
        public string? idUsuario { get; set; } = string.Empty;
        [StringLength(50, ErrorMessage = "El nombre del rol no puede tener más de 50 caracteres.")]
        public string? IdRol { get; set; } = string.Empty;
        public string? NombreRol { get; set; } = string.Empty;


        // Método para realizar validaciones personalizadas
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(IdRol) && string.IsNullOrWhiteSpace(NombreRol))
            {
                yield return new ValidationResult(
                    "Debe proporcionar al menos el Id del rol o el nombre del rol.",
                    new[] { nameof(IdRol), nameof(NombreRol) }
                );
            }
        }

    }
}
