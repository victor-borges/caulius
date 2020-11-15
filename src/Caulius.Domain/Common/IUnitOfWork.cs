using System;
using System.Threading;
using System.Threading.Tasks;

namespace Caulius.Domain.Common
{
    public interface IUnitOfWork : IDisposable
    {        
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        public bool EnsureDatabaseCreated();
    }
}
