namespace Data.Entities.Abstractions;

public record BaseEntity : IEntity
{
    public int Id { get; set; }
}