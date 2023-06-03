using Database.Context;

namespace Database.Repositories.Concrete
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly InstagramContext _context;

        public UnitOfWork(InstagramContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveChangesAsync()
        {
            var result = await _context.SaveChangesAsync() > 0;

            _context.ChangeTracker.Clear();

            return result;
        }
    }
}