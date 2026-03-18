using WF.Domain.Dtos;

namespace WF.Cqrs.Crud.Domain;

// This is probably too much interface marking, lol
public interface ISummaryDto : IDto;

// TODO somewhere add a base mapper class that prevents me from
// - specifying twice all the mappings defined in a summary
// - requiring me another class for mapping the creationDto
// something like BaseMappingConfiguration<TEntity, TCreationDto, TSummaryDto, TDetailDto>