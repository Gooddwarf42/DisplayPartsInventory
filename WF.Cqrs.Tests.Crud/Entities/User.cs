using WF.Data.Configurators;
using WF.Data.Entities;

namespace WF.Cqrs.Tests.Crud.Entities;

public sealed record User : BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }
}

internal sealed class UserConfigurator : BaseEntityConfigurator<User>;