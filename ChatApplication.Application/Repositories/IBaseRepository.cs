namespace ChatApplication.Application.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task Create_Async(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<TEntity> Get_Async(Guid id);
        Task<List<TEntity>> GetAll_Async();
    }
}
