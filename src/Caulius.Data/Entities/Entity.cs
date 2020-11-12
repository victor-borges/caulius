using System;

namespace Caulius.Data.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
