using Dominio.Usuarios;
using Servicio.Usuarios.UsuariosDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.Usuarios
{
    public interface IServicioUsuario
    {
        public Task<Usuario> ObtenerPorId(string id);
        // Obtener todos los elementos con soporte de paginación y cancelación
        public Task<IEnumerable<Usuario>> ObtenerTodos();
        public Task<Usuario> Crear(CrearUsuarioDTO usuario);
        public Task<Usuario> Actualizar(string id, ActualizarUsuarioDTO usuario);
        public Task<bool> Eliminar(string id);
    }
}
