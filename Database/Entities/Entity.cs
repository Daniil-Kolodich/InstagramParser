namespace Database.Entities;

public abstract class Entity
{
    public int Id { get; private set; }
    internal DateTime CreatedWhen { get; set; }
    internal DateTime? UpdatedWhen { get; set; } = null;
    internal bool IsDeleted { get; set; }
}