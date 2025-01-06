using Dominio.Documentos;
using Dominio;
using Dominio.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.Token
{
    public interface IRepositorioRefreshToken : IRepositorioCRUD<RefreshToken>
    {
        public Task<Resultado<RefreshToken>> ObtenerPorUsuarioIdAsync(string usuarioId);
        public Task<Resultado<IEnumerable<RefreshToken>>> ObtenerTodosAsync();
    }
}
