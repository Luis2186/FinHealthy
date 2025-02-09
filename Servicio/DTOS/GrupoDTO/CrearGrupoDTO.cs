using Dominio.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.DTOS.GrupoDTO
{
    public class CrearGrupoDTO : ActualizarGrupoDTO
    {

        public string? UsuarioAdministradorId { get; set; }
        [Required(ErrorMessage = "El codigo de acceso es un campo requerido, por favor ingreselo")]
        public string? CodigoAcceso { get; set; }

        public bool EsValido()
        {
            return this.Nombre != null && this.Descripcion != "" && this.CodigoAcceso != "";
        }

    }
}
