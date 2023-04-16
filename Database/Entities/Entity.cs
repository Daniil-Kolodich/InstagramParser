namespace Database.Entities;

public abstract class Entity
{
    public int Id { get; private set; }
    public DateTime CreatedWhen { get; set; }
    public DateTime? UpdatedWhen { get; set; } = null;
}