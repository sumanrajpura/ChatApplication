namespace ChatApplication.Application.Repositories
{
    public interface IUnitOfWork
    {
        Task Save_Async();
    }
}
