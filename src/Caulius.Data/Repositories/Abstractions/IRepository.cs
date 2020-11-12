using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Caulius.Data.Entities;

namespace Caulius.Data.Repositories.Abstractions
{
    public interface IReadOnlyRepository<TEntity> where TEntity : Entity
    {
        TEntity Find(Guid id);
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate = null);
    }

    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : Entity
    {
        TEntity Create(TEntity entity);
        TEntity Update(TEntity entity);
        TEntity Delete(TEntity entity);
    }
}
