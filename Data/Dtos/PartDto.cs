using Data.Dtos.Abstractions;

namespace Data.Dtos;

public record PartDtoSummary : BaseDto
{
    public required string Type { get; set; } //TODO: make enum
    public required string Location { get; set; }
    public required int Quantity { get; set; }
    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public required decimal Size1 { get; set; }
    public decimal? Size2 { get; set; }
    public string? Notes { get; set; }
}

public sealed record PartDto : PartDtoSummary;