using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.Usuarios.UsuariosDTO
{
    public class ActualizarUsuarioDTO
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede tener más de 50 caracteres.")]
        public string? NombreDeUsuario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50, ErrorMessage = "El apellido no puede tener más de 50 caracteres.")]
        public string? Apellido { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "El formato del teléfono no es válido.")]
        public string? Telefono { get; set;}

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "La fecha no tiene un formato válido.")]
        [CustomValidation(typeof(ActualizarUsuarioDTO), nameof(ValidarEdadMinima))]
        public DateTime FechaDeNacimiento { get; set; }
        public bool Activo { get; set; }
        public string? Token { get; set; }

        // Validador personalizado para la fecha de nacimiento
        public static ValidationResult? ValidarEdadMinima(DateTime fecha, ValidationContext context)
        {
            var edadMinima = 18;
            if (fecha > DateTime.Now.AddYears(-edadMinima))
            {
                return new ValidationResult($"El usuario debe ser mayor de {edadMinima} años.");
            }

            return ValidationResult.Success;
        }
    }
}
