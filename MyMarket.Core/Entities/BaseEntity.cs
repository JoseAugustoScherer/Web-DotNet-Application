namespace MyMarket.Core.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
}