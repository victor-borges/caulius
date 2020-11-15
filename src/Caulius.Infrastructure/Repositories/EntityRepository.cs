using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Caulius.Domain.Common;

namespace Caulius.Infrastructure.Repositories
{
    public class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly CauliusContext _context;

        public IQueryable<TEntity> Query => _context.Set<TEntity>();

        public IUnitOfWork UnitOfWork => _context;

        public EntityRepository(CauliusContext context)
        {
            _context = context;
        }

        public Task<TEntity?> FindAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _context.FindAsync<TEntity?>(new object[] { id }, cancellationToken).AsTask();
        }

        public void Add(TEntity entity)
        {
            _context.Add(entity);
        }

        public void Update(TEntity entity)
        {
            _context.Update(entity);
        }

        public void Remove(TEntity entity)
        {
            _context.Remove(entity);
        }
    }
}
