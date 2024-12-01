using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _currentTransaction;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;   
        }
        public async Task ConfirmarTransaccionAsync()
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("No hay transacción activa para confirmar.");

            await _currentTransaction.CommitAsync();
        }

        public async Task IniciarTransaccionAsync()
        {
            // Verifica si ya existe una transacción activa
            if (_currentTransaction != null)
                throw new InvalidOperationException("Ya existe una transacción activa.");

            _currentTransaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task RevertirTransaccionAsync()
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("No hay transacción activa para revertir.");

            await _currentTransaction.RollbackAsync();
        }

        // Guarda los cambios en el contexto (incluyendo la transacción activa)
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Dispose de la transacción y el contexto
        public void Dispose()
        {
            _context.Dispose();
            _currentTransaction?.Dispose();
        }
    }
}
