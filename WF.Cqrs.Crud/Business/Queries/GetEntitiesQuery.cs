using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WF.Cqrs.Crud.Domain;
using WF.Cqrs.Handlers;
using WF.Cqrs.Operations;
using WF.Data.Context;
using WF.Data.Entities;

namespace WF.Cqrs.Crud.Business.Queries;

public sealed record GetEntitiesQuery<TEntity, TSummaryDto>(Expression<Func<TEntity, bool>>? Filter = null) : IQuery<List<TSummaryDto>>
    where TEntity : BaseEntity
    where TSummaryDto : BaseSummaryDto;

file sealed class GetEntitiesQueryHandler<TEntity, TSummaryDto>(ApplicationDbContext dbContext, IMapper mapper) : IOperationHandler<GetEntitiesQuery<TEntity, TSummaryDto>, List<TSummaryDto>>
    where TEntity : BaseEntity
    where TSummaryDto : BaseSummaryDto
{
    public ValueTask<List<TSummaryDto>> HandleAsync(GetEntitiesQuery<TEntity, TSummaryDto> operation, CancellationToken cancellationToken = default)
    {
        var queryable = dbContext.Set<TEntity>().AsQueryable();
        if (operation.Filter is not null)
        {
            queryable = queryable.Where(operation.Filter);
        }

        var projectedQueryable = mapper.ProjectTo<TSummaryDto>(queryable);
        return new ValueTask<List<TSummaryDto>>(projectedQueryable.ToListAsync(cancellationToken));
    }
}