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

[TestSubject(typeof(DeleteEntityCommand<>))]
public class DeleteEntityCommandTest : BaseTest
{
    [Fact]
    public async Task DeletesEntity()
    {
        // Arrange
        var idToDelete = SeededUsers[0].Id;

        // Act
        await Mediator.RunAsync(new DeleteEntityCommand<User>(idToDelete));

        // Assert
        DbContext.ChangeTracker.Clear(); // So we read exactly what is in the database
        var allUsers = await DbContext.Set<User>().ToListAsync();
        Assert.Equal(1, allUsers.Count);

        Assert.All(allUsers, e => Assert.NotEqual(idToDelete, e.Id));
    }

    [Fact]
    public async Task Should_Throw_When_IdIsInvalid()
    {
        // Arrange
        var idToDelete = Guid.NewGuid();

        // Act // Assert
        await Assert.ThrowsAsync<Exception>(() => Mediator.RunAsync(new DeleteEntityCommand<User>(idToDelete)).AsTask());
    }
}