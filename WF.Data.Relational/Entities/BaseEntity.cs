namespace WF.Data.Relational.Entities;

public abstract record BaseEntity : IEntity
{
    public Guid Id { get; set; }
}