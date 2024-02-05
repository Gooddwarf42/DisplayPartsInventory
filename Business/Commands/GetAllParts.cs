using Cqrs.Handlers;
using Cqrs.Operations;
using Data.Dtos;
using Data.Entities;
using Data.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Business.Commands;

internal sealed class GetAllParts : ICommand<IEnumerable<PartDtoSummary>> { }

internal sealed class GetAllPartsHandler(ApplicationDbContext dbContext) : ICommandHandler<GetAllParts, IEnumerable<PartDtoSummary>>
{
    public async ValueTask<IEnumerable<PartDtoSummary>> HandleAsync(GetAllParts command, CancellationToken cancellationToken)
    {
        var entityList = await dbContext.Set<Part>()
            .ToListAsync(cancellationToken);
        return entityList.Select(Map);
    }

    private PartDtoSummary Map(Part part) =>
        //TODO USe autpmapper
        new()
        {
            Type = "mimmo",
            Location = "locescion",
            Quantity = 4,
            Size1 = 11.1m
        };
}