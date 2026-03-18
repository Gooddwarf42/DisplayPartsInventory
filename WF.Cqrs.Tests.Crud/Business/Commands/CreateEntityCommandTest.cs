using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using WF.Cqrs.Crud.Business.Commands;
using WF.Cqrs.Tests.Crud.Abstractions;
using WF.Cqrs.Tests.Crud.Dtos;
using WF.Cqrs.Tests.Crud.Entities;
using Xunit;

namespace WF.Cqrs.Tests.Crud.Business.Commands;

[TestSubject(typeof(CreateEntityCommand<,>))]
public class CreateEntityCommandTest : BaseTest
{
    [Fact]
    public async Task CreatesNewEntity()
    {
        // Arrange
        var creationDto = new UserCreationDto()
        {
            Age = 25,
            Name = "Jonny",
            Surname = "Bravo",
        };

        // Act
        var createdId = await Mediator.RunAsync(new CreateEntityCommand<User, UserCreationDto>(creationDto));

        // Assert
        DbContext.ChangeTracker.Clear(); // So we read exactly what is in the database
        var allUsers = await DbContext.Set<User>().ToListAsync();
        Assert.Equal(3, allUsers.Count);

        var createdUser = allUsers.SingleOrDefault(e => e.Id == createdId);
        Assert.NotNull(createdUser);
        Assert.Equal(new User{Id = createdId, Age = 25, Name = "Jonny", Surname = "Bravo"}, createdUser);
    }
}