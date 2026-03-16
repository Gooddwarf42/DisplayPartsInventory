using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WF.Data.Configurators;

namespace Data.Configurators;

public class PartEntityConfigurator : BaseEntityConfigurator<Part>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Part> builder)
    {
        builder.Property(e => e.Notes)
            .HasDefaultValue("defaultNote");
    }
}