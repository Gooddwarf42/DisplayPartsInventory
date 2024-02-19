using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Mapper.Abstractions.Configurators;
using Data.Dtos;
using Data.Entities;

namespace Business.Mapper.Configurators;

internal sealed class PartMappingConfiguration : MappingConfiguration<Part, PartDto>
{
    protected override void Configure(IMappingExpression<Part, PartDto> expression)
    {
        expression.ForMember
        (
            dto => dto.Size2,
            options => options.MapFrom(entity => entity.Size1 * 100)
        );
    }
    protected override void Configure(IMappingExpression<PartDto, Part> expression)
    {
        expression.ForMember
        (
            entity => entity.Size2,
            options => options.MapFrom(dto => dto.Size1 / 100)
        );
    }
}
