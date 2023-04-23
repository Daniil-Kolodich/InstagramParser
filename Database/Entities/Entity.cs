namespace Database.Entities;

public abstract class Entity
{
    // TODO: check may be init is working too
    public int Id { get; private set; }
    public DateTime CreatedWhen { get; internal set; }
    public DateTime? UpdatedWhen { get;  internal set; } = null;
    public bool IsDeleted { get; internal set; }
}