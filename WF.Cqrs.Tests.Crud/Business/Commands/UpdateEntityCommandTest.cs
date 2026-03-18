using System;
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

[TestSubject(typeof(UpdateEntityCommand<,>))]
public class UpdateEntityCommandTest : BaseTest
{
    [Fact]
    public async Task UpdatesEntity()
    {
        // Arrange
        var userToUpdate = SeededUsers[0];
        const string updatedSurname = "UpdatedData";

        var detailDto = new UserDetailDto
        {
            Id = userToUpdate.Id,
            Age = userToUpdate.Age,
            Name = userToUpdate.Name,
            Surname = updatedSurname,
        };

        // Act
        var updatedId = await Mediator.RunAsync(new UpdateEntityCommand<User, UserDetailDto>(userToUpdate.Id, detailDto));

        // Assert
        DbContext.ChangeTracker.Clear(); // So we read exactly what is in the database
        var allUsers = await DbContext.Set<User>().ToListAsync();
        Assert.Equal(2, allUsers.Count);

        var updatedUser = allUsers.SingleOrDefault(e => e.Id == updatedId);
        Assert.NotNull(updatedUser);
        Assert.Equal(new User { Id = userToUpdate.Id, Age = userToUpdate.Age, Name = userToUpdate.Name, Surname = updatedSurname }, updatedUser);
    }

    [Fact]
    public async Task Should_Throw_When_IdIsInvalid()
    {
        // Arrange
        var userToUpdate = SeededUsers[0];
        const string updatedSurname = "UpdatedData";

        var detailDto = new UserDetailDto
        {
            Id = userToUpdate.Id,
            Age = userToUpdate.Age,
            Name = userToUpdate.Name,
            Surname = updatedSurname,
        };

        // Act // Assert
        await Assert.ThrowsAsync<Exception>(() => Mediator.RunAsync(new UpdateEntityCommand<User, UserDetailDto>(Guid.NewGuid(), detailDto)).AsTask());
    }
}