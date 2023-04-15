using Database.Context;

namespace Database.UnitOfWork.Concrete;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly InstagramContext _context;

    public UnitOfWork(InstagramContext context)
    {
        _context = context;
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}