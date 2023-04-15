namespace Database.UnitOfWork;

public interface IUnitOfWork
{
    Task<bool> SaveChanges();
}