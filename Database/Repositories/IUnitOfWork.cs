namespace Database.Repositories;

public interface IUnitOfWork
{
    Task<bool> SaveChangesAsync();
}