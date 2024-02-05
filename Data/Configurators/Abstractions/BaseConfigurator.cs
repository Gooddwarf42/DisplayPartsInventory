using Data.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurators.Abstractions;

public class BaseConfigurator<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class, IEntity
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder
            .ToTable(nameof(TEntity))
            .HasKey(e => e.Id);

        ConfigureEntity(builder);
    }

    protected virtual void ConfigureEntity(EntityTypeBuilder<TEntity> builder) { }
}