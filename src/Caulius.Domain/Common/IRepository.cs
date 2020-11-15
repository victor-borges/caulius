using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Caulius.Domain.Common
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        public Task<TEntity?> FindAsync(Guid id, CancellationToken cancellationToken = default);
        public void Add(TEntity entity);
        public void Update(TEntity entity);
        public void Remove(TEntity entity);

        public IQueryable<TEntity> Query { get; }
        public IUnitOfWork UnitOfWork { get; }
    }
}
