namespace Database.Repository;

public interface IUnitOfWork
{
    Task<bool> SaveChanges();
}