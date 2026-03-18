using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using WF.Cqrs.Crud.Business.Queries;
using WF.Cqrs.Tests.Crud.Abstractions;
using WF.Cqrs.Tests.Crud.Dtos;
using WF.Cqrs.Tests.Crud.Entities;
using Xunit;

namespace WF.Cqrs.Tests.Crud.Business.Queries;

[TestSubject(typeof(GetEntityQuery<,>))]
public class GetEntityQueryTest : BaseTest
{
    [Fact]
    public async Task GetsEntity()
    {
        // Arrange
        var entityToRetrieve = SeededUsers[0];

        var expected = new UserDetailDto
        {
            Id = entityToRetrieve.Id,
            Name = entityToRetrieve.Name,
            Surname = entityToRetrieve.Surname,
            Age = entityToRetrieve.Age,
            FullName = $"{entityToRetrieve.Name} {entityToRetrieve.Surname}",
        };

        // Act
        var result = await Mediator.RunAsync(new GetEntityQuery<User, UserDetailDto>(entityToRetrieve.Id));

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task Should_Throw_When_NotFound()
    {
        // Arrange

        // Act // Assert
        await Assert.ThrowsAsync<Exception>(() => Mediator.RunAsync(new GetEntityQuery<User, UserDetailDto>(Guid.NewGuid())).AsTask());
    }
}