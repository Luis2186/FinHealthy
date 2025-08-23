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
using System.Threading;

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

        public async Task<Resultado<RefreshToken>> ObtenerPorToken(string token)
        {
            try
            {
                var entidad = await _dbContext.RefreshTokens
                    .FirstOrDefaultAsync(doc => doc.Token == token &&
                                         doc.FechaExpiracion > DateTime.UtcNow &&
                                        !doc.Revocado);

                return entidad == null
                    ? Resultado<RefreshToken>.Failure(ErroresCrud.ErrorDeCreacion(typeof(RefreshToken).Name))
                    : Resultado<RefreshToken>.Success(entidad);
            }
            catch (Exception ex)
            {
                return Resultado<RefreshToken>.Failure(ErroresCrud.ErrorDeExcepcion($"{typeof(RefreshToken).Name}.ObtenerPorToken", ex.Message));
            }
        }

        public async Task<Resultado<RefreshToken>> ObtenerPorToken(string token, CancellationToken cancellationToken)
        {
            try
            {
                var entidad = await _dbContext.RefreshTokens
                    .FirstOrDefaultAsync(doc => doc.Token == token &&
                                         doc.FechaExpiracion > DateTime.UtcNow &&
                                        !doc.Revocado, cancellationToken);

                return entidad == null
                    ? Resultado<RefreshToken>.Failure(ErroresCrud.ErrorDeCreacion(typeof(RefreshToken).Name))
                    : Resultado<RefreshToken>.Success(entidad);
            }
            catch (Exception ex)
            {
                return Resultado<RefreshToken>.Failure(ErroresCrud.ErrorDeExcepcion($"{typeof(RefreshToken).Name}.ObtenerPorToken", ex.Message));
            }
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

        public async Task<Resultado<Usuario>> ObtenerUsuarioPorToken(string token)
        {
            try
            {
                var entidad = await _dbContext.RefreshTokens
                    .FirstOrDefaultAsync(doc => doc.Token == token);

                var usuario = entidad.Usuario;

                return entidad == null
                    ? Resultado<Usuario>.Failure(ErroresCrud.ErrorDeCreacion(typeof(RefreshToken).Name))
                    : Resultado<Usuario>.Success(usuario);
            }
            catch (Exception ex)
            {
                return Resultado<Usuario>.Failure(ErroresCrud.ErrorDeExcepcion($"{typeof(RefreshToken).Name}.ObtenerUsuarioPorToken", ex.Message));
            }
        }

        public async Task<Resultado<bool>> Revocar(RefreshToken refreshToken)
        {
            try
            {
                // Revocar el token anterior
                refreshToken.Revocado = true;
                _dbContext.RefreshTokens.Update(refreshToken);

                // Guardar cambios
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion($"{typeof(RefreshToken).Name}.Revocar", ex.Message));
            }
        }

        public async Task<Resultado<bool>> Revocar(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            try
            {
                refreshToken.Revocado = true;
                _dbContext.RefreshTokens.Update(refreshToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion($"{typeof(RefreshToken).Name}.Revocar", ex.Message));
            }
        }

        public async Task<Resultado<bool>> RevocarTokensAntiguos(string usuarioId,int cantidadSesionesActivas)
        {
            try
            {
                var tokensAntiguos = _dbContext.RefreshTokens
                    .Where(rt => rt.UsuarioId == usuarioId &&
                                 rt.FechaExpiracion > DateTime.UtcNow &&
                                !rt.Revocado)
                    .OrderByDescending(rt => rt.FechaExpiracion)
                    .Skip(cantidadSesionesActivas);
                
                foreach (var token in tokensAntiguos)
                {
                    token.Revocado = true;
                    _dbContext.RefreshTokens.Update(token);
                }

                return await _dbContext.SaveChangesAsync() >= 1;
                
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion($"{typeof(RefreshToken).Name}.RevocarTokensAntiguos", ex.Message));
                throw;
            }
        }

        public async Task<Resultado<bool>> RevocarYCrearNuevo(RefreshToken refreshTokenAnterior, RefreshToken nuevoRefreshToken)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // Revocar el token anterior
                    refreshTokenAnterior.Revocado = true;
                    _dbContext.RefreshTokens.Update(refreshTokenAnterior);

                    // Crear el nuevo token
                    await _dbContext.RefreshTokens.AddAsync(nuevoRefreshToken);

                    // Guardar cambios
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        public async Task<Resultado<bool>> RevocarYCrearNuevo(RefreshToken refreshTokenAnterior, RefreshToken nuevoRefreshToken, CancellationToken cancellationToken)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    refreshTokenAnterior.Revocado = true;
                    _dbContext.RefreshTokens.Update(refreshTokenAnterior);
                    await _dbContext.RefreshTokens.AddAsync(nuevoRefreshToken, cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return false;
                }
            }
        }
    }
}
