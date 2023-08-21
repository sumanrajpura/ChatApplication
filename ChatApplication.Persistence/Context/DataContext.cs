using ChatApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.Persistence.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Agent> Agents { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ChatRequest> ChatRequests { get; set; }
        public DbSet<Chat> Chats { get; set; }

    }
}
