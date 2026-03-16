using AutoMapper;
using WF.Cqrs.Crud.Domain;
using WF.Cqrs.Handlers;
using WF.Cqrs.Operations;
using WF.Data.Context;
using WF.Data.Entities;

namespace WF.Cqrs.Crud.Business.Commands;

public sealed record UpdateEntityCommand<TEntity, TDetailDto>(Guid Id, TDetailDto DetailDto) : ICommand<Guid>
    where TEntity : BaseEntity
    where TDetailDto : DetailDto;

file sealed class UpdateEntityCommandHandler<TEntity, TDetailDto>(ApplicationDbContext dbContext, IMapper mapper) : IOperationHandler<UpdateEntityCommand<TEntity, TDetailDto>, Guid>
    where TEntity : BaseEntity
    where TDetailDto : DetailDto
{
    public async ValueTask<Guid> HandleAsync(UpdateEntityCommand<TEntity, TDetailDto> operation, CancellationToken cancellationToken = default)
    {
        var storedEntity = dbContext.Set<TEntity>()
            .SingleOrDefault(e => e.Id == operation.Id);

        if (storedEntity is null)
        {
            // TODO create decent exceptions
            throw new Exception($"{typeof(TEntity).Name} with id {operation.Id} not found");
        }

        mapper.Map(operation.DetailDto, storedEntity);

        await dbContext.SaveChangesAsync(cancellationToken);
        return storedEntity.Id;
    }
}