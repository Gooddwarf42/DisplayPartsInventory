using AutoMapper;
using WF.Cqrs.Crud.Domain;
using WF.Cqrs.Handlers;
using WF.Cqrs.Operations;
using WF.Data.Context;
using WF.Data.Entities;

namespace WF.Cqrs.Crud.Business.Commands;

public sealed record DeleteEntityCommand<TEntity>(Guid Id) : ICommand
    where TEntity : BaseEntity;

file sealed class DeleteEntityCommandHandler<TEntity>(ApplicationDbContext dbContext) : IOperationHandler<DeleteEntityCommand<TEntity>>
    where TEntity : BaseEntity
{
    public async ValueTask HandleAsync(DeleteEntityCommand<TEntity> operation, CancellationToken cancellationToken = default)
    {
        var storedEntity = dbContext.Set<TEntity>()
            .SingleOrDefault(e => e.Id == operation.Id);

        if (storedEntity is null)
        {
            // TODO create decent exceptions
            throw new Exception($"{typeof(TEntity).Name} with id {operation.Id} not found");
        }

        dbContext.Remove(storedEntity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}