using ChatApplication.Application.Repositories;
using ChatApplication.Domain.Entities;
using ChatApplication.Persistence.Context;

namespace ChatApplication.Persistence.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(DataContext context) : base(context)
        {
        }
    }
}
