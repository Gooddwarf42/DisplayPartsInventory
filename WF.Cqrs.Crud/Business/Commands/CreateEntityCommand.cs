using AutoMapper;
using WF.Cqrs.Crud.Domain;
using WF.Cqrs.Handlers;
using WF.Cqrs.Operations;
using WF.Data.Context;
using WF.Data.Entities;

namespace WF.Cqrs.Crud.Business.Commands;

public sealed record CreateEntityCommand<TEntity, TCreationDto>(TCreationDto CreationDto) : ICommand<Guid>
    where TEntity : BaseEntity
    where TCreationDto : class, ICreationDto;

file sealed class CreateEntityCommandHandler<TEntity, TCreationDto>(ApplicationDbContext dbContext, IMapper mapper) : IOperationHandler<CreateEntityCommand<TEntity, TCreationDto>, Guid>
    where TEntity : BaseEntity
    where TCreationDto : class, ICreationDto
{
    public async ValueTask<Guid> HandleAsync(CreateEntityCommand<TEntity, TCreationDto> operation, CancellationToken cancellationToken = default)
    {
        var entity = mapper.Map<TEntity>(operation.CreationDto);
        
        dbContext.Add(entity);
        if (entity.Id == Guid.Empty)
        {
            entity.Id = Guid.NewGuid();
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}