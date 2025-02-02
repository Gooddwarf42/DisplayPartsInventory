namespace WF.Domain.Dtos;

public record BaseDto : IDto
{
    public Guid Id { get; set; }
}