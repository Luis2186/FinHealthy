using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios
{
    public interface IUnitOfWork
    {
        public Task IniciarTransaccionAsync();
        public Task ConfirmarTransaccionAsync();
        public Task RevertirTransaccionAsync();
        public Task<int> SaveChangesAsync();
        public void Dispose();

    }
}
