using Cqrs.Commands;
using Data.Dtos;

namespace Business.Commands;

internal sealed class GetAllParts : ICommand<IEnumerable<PartDtoSummary>> { }

// TODO: commandExecutor