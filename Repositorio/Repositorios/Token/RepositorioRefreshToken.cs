using Dominio;
using Dominio.Documentos;
using Dominio.Errores;
using Dominio.Usuarios;
using Microsoft.EntityFrameworkCore;
using Repositorio.Repositorios.Validacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.Token
{
    public class RepositorioRefreshToken : RepositorioCRUD<RefreshToken>, IRepositorioRefreshToken
    {
        private readonly ApplicationDbContext _dbContext;
        public RepositorioRefreshToken(ApplicationDbContext context, IValidacion<RefreshToken> validacion)
            : base(context, validacion)
        {
            _dbContext = context;
        }
        
        public async Task<Resultado<RefreshToken>> ObtenerPorUsuarioIdAsync(string idUsuario)
        {
            try
            {
                var entidad = await _dbContext.RefreshTokens
                    .FirstOrDefaultAsync(doc => doc.UsuarioId == idUsuario);

                return entidad == null
                    ? Resultado<RefreshToken>.Failure(ErroresCrud.ErrorDeCreacion(typeof(RefreshToken).Name))
                    : Resultado<RefreshToken>.Success(entidad);
            }
            catch (Exception ex)
            {
                return Resultado<RefreshToken>.Failure(ErroresCrud.ErrorDeExcepcion($"{typeof(RefreshToken).Name}.ObtenerPorIdAsync", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<RefreshToken>>> ObtenerTodosAsync()
        {
            try
            {
                var categorias = _dbContext.RefreshTokens
                    .ToList();

                return Resultado<IEnumerable<RefreshToken>>.Success(categorias);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<RefreshToken>>.Failure(ErroresCrud.ErrorDeExcepcion("FIND_ALL", ex.Message));
            }
        }
    }
}
