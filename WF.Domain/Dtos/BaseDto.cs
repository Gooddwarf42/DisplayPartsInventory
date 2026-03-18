namespace WF.Domain.Dtos;

public abstract record BaseDto : IDto
{
    public Guid Id { get; set; }
}