using System;

namespace Caulius.Domain.Common
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
