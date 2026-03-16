using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WF.Cqrs.Crud.Domain;
using WF.Cqrs.Handlers;
using WF.Cqrs.Operations;
using WF.Data.Context;
using WF.Data.Entities;

namespace WF.Cqrs.Crud.Business.Queries;

public sealed record GetEntityQuery<TEntity, TDetailDto>(Guid Id) : IQuery<TDetailDto>
    where TEntity : BaseEntity
    where TDetailDto : DetailDto;

file sealed class GetEntityQueryHandler<TEntity, TDetailDto>(ApplicationDbContext dbContext, IMapper mapper) : IOperationHandler<GetEntityQuery<TEntity, TDetailDto>, TDetailDto>
    where TEntity : BaseEntity
    where TDetailDto : DetailDto
{
    public async ValueTask<TDetailDto> HandleAsync(GetEntityQuery<TEntity, TDetailDto> operation, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.Set<TEntity>()
            .SingleOrDefaultAsync(e => e.Id == operation.Id, cancellationToken);

        if (entity is null)
        {
            // TODO create decent exceptions
            throw new Exception($"{typeof(TEntity).Name} with id {operation.Id} not found");
        }

        return mapper.Map<TDetailDto>(entity);
    }
}