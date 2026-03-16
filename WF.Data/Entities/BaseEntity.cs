namespace WF.Data.Entities;

public abstract record BaseEntity : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
}