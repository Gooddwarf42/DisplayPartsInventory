namespace Data.Dtos.Abstractions;

public record BaseDto : IDto
{
    public int Id { get; set; }
}