using ChatApplication.Application.Repositories;
using ChatApplication.Persistence.Context;

namespace ChatApplication.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }
        public async Task Save_Async()
        {
            await _context.SaveChangesAsync();
        }
    }
}
