namespace WF.Data.Relational.Entities;

public record BaseEntity : IEntity
{
    public Guid Id { get; set; }
}