using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using WF.Cqrs.Crud.Business.Queries;
using WF.Cqrs.Tests.Crud.Abstractions;
using WF.Cqrs.Tests.Crud.Dtos;
using WF.Cqrs.Tests.Crud.Entities;
using Xunit;

namespace WF.Cqrs.Tests.Crud.Business.Queries;

[TestSubject(typeof(GetEntitiesQuery<,>))]
public class GetEntitiesQueryTest : BaseTest
{
    [Fact]
    public async Task GetsEntities()
    {
        // Arrange
        var expected = SeededUsers.Select(e => new UserSummaryDto { Id = e.Id, Name = e.Name, Surname = e.Surname }).ToList();

        // Act
        var result = await Mediator.RunAsync(new GetEntitiesQuery<User, UserSummaryDto>());

        // Assert
        Assert.Equal(expected.Count, result.Count);
        Assert.Equal(expected.OrderBy(e => e.Id), result.OrderBy(e => e.Id));
    }
}