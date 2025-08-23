using Dominio.Documentos;
using Dominio;
using Dominio.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Repositorio.Repositorios.Token
{
    public interface IRepositorioRefreshToken : IRepositorioCRUD<RefreshToken>
    {
        public Task<Resultado<RefreshToken>> ObtenerPorUsuarioIdAsync(string usuarioId);
        public Task<Resultado<IEnumerable<RefreshToken>>> ObtenerTodosAsync();
        public Task<Resultado<RefreshToken>> ObtenerPorToken(string token, CancellationToken cancellationToken);
        public Task<Resultado<Usuario>> ObtenerUsuarioPorToken(string token);
        public Task<Resultado<bool>> RevocarYCrearNuevo(RefreshToken refreshTokenAnterior, RefreshToken nuevoRefreshToken, CancellationToken cancellationToken);
        public Task<Resultado<bool>> Revocar(RefreshToken refreshToken, CancellationToken cancellationToken);
        public Task<Resultado<bool>> RevocarTokensAntiguos(string usuarioId, int cantidadSesionesActivas);
    }
}
