using ChatApplication.Application.Repositories;
using ChatApplication.Domain.Common;
using ChatApplication.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.Persistence.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly DataContext Context;

        public BaseRepository(DataContext context)
        {
            Context = context;
        }

        public async Task Create_Async(TEntity entity)
        {
            await Context.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            Context.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            Context.Update(entity);
        }

        public Task<TEntity> Get_Async(Guid id)
        {
            return Context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<TEntity>> GetAll_Async()
        {
            return Context.Set<TEntity>().ToListAsync();
        }
    }
}
