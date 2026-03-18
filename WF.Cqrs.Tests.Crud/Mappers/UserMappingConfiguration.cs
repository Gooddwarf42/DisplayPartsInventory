using AutoMapper;
using WF.Cqrs.Tests.Crud.Dtos;
using WF.Cqrs.Tests.Crud.Entities;
using WF.Mapper.Configurators;
using WF.Mapper.Extensions;

namespace WF.Cqrs.Tests.Crud.Mappers;

internal class UserCreationMappingConfiguration : MappingConfiguration<User, UserCreationDto>;

internal class UserSummaryMappingConfiguration : MappingConfiguration<User, UserSummaryDto>;

internal class UserDetailMappingConfiguration : MappingConfiguration<User, UserDetailDto>
{
    protected override void Configure(IMappingExpression<User, UserDetailDto> expression)
    {
        expression.Bind(dto => dto.FullName, e => $"{e.Name} {e.Surname}");
    }
}