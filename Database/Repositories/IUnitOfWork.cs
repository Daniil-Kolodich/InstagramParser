namespace Database.Repositories;

public interface IUnitOfWork
{
    Task<bool> SaveChanges();
}