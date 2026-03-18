using WF.Cqrs.Crud.Domain;

namespace WF.Cqrs.Tests.Crud.Dtos;

internal record UserSummaryDto : BaseSummaryDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
}

internal record UserDetailDto : UserSummaryDto, IDetailDto
{
    public string FullName { get; set; }
    public int Age { get; set; }
}

internal sealed record UserCreationDto : ICreationDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }
}