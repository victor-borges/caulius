using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Caulius.Data.Entities;

namespace Caulius.Data.Repositories.Abstractions
{
    public interface IReadOnlyAsyncRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> Find(Guid id);
        Task<IEnumerable<TEntity>> FindAll(Expression<Func<TEntity, bool>> predicate = null);
    }

    public interface IAsyncRepository<TEntity> : IReadOnlyAsyncRepository<TEntity> where TEntity : Entity
    {
        Task Create(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(TEntity entity);
    }
}
