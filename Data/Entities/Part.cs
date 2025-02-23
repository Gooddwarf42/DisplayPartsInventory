﻿using Data.Entities.Abstractions;

namespace Data.Entities;

public sealed record Part : BaseEntity
{
    public required string Type { get; set; } //TODO: make enum
    public required string Location { get; set; }
    public required int Quantity { get; set; }
    public required decimal Size1 { get; set; }
    public decimal? Size2 { get; set; }
    public string? Notes { get; set; }
}