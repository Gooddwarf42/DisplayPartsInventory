using AutoMapper;
using Data.Dtos;
using Data.Entities;
using Mapper.Configurators;
using Mapper.Extensions;

namespace Business.Mapper.Configurators;

internal sealed class PartMappingConfiguration : MappingConfiguration<Part, PartDto>
{
    protected override void Configure(IMappingExpression<Part, PartDto> expression)
    {
        expression.Bind(dto => dto.Size2, entity => entity.Size1 * 100);
    }
    protected override void Configure(IMappingExpression<PartDto, Part> expression)
    {
        expression.Bind(entity => entity.Size2, dto => dto.Size1 / 100);
    }
}
