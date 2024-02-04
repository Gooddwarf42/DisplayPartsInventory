namespace Data.Entities.Abstractions;

public record Entity : IEntity
{
    public int Id { get; set; }
}