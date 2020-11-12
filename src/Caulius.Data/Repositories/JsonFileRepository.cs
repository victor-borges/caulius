using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using Caulius.Data.Entities;
using Caulius.Data.Repositories.Abstractions;

namespace Caulius.Data.Repositories
{
    public class JsonFileRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : Entity
    {
        private readonly IEnumerable<TEntity> _entities;

        public JsonFileRepository(string filePath)
        {
            _entities = JsonSerializer.Deserialize<IEnumerable<TEntity>>(File.ReadAllText(filePath));
        }

        public TEntity Find(Guid id)
        {
            return _entities.Single(entity => entity.Id == id);
        }

        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate = null)
        {
            predicate ??= _ => true;
            return _entities.AsQueryable().Where(predicate);
        }
    }
}
