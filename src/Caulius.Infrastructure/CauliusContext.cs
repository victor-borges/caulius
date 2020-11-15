using Caulius.Domain.Aggregates.TextArt;
using Caulius.Domain.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Caulius.Infrastructure
{
    public class CauliusContext : DbContext, IUnitOfWork
    {
        [UsedImplicitly]
        public DbSet<TextCommand> TextCommands => Set<TextCommand>();

        public CauliusContext(DbContextOptions<CauliusContext> options)
            : base(options)
        { }

        public bool EnsureDatabaseCreated()
        {
            return Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CauliusContext).Assembly);
        }
    }
}
