using Caulius.Domain.Aggregates.CommandAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Caulius.Infrastructure.Configurations;

public class TextCommandConfiguration : IEntityTypeConfiguration<TextCommand>
{
    public void Configure(EntityTypeBuilder<TextCommand> builder)
    {
        builder.ToContainer("TextCommands");

        builder.HasKey(t => t.Id);
        builder.HasPartitionKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasConversion(new GuidToStringConverter())
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Created).ValueGeneratedOnAdd();
        builder.Property(t => t.Updated).ValueGeneratedOnUpdate();
        builder.Property(t => t.Command).IsRequired();
        builder.HasIndex(t => t.Command).IsUnique(false);
        builder.Property(t => t.Text).IsRequired();
    }
}