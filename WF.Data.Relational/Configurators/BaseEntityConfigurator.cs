using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WF.Data.Relational.Entities;

namespace WF.Data.Relational.Configurators;

public abstract class BaseEntityConfigurator<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class, IEntity
{
    protected virtual string TableName => typeof(TEntity).Name.Pluralize();
    protected virtual string? SchemaName => null;

    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder
            .ToTable(TableName, SchemaName)
            .HasKey(e => e.Id);

        ConfigureEntity(builder);
    }

    protected virtual void ConfigureEntity(EntityTypeBuilder<TEntity> builder) { }
}