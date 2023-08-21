using ChatApplication.Application.Repositories;
using ChatApplication.Domain.Entities;
using ChatApplication.Persistence.Context;

namespace ChatApplication.Persistence.Repositories
{
    public class ChatRepository : BaseRepository<Chat>, IChatRepository
    {
        public ChatRepository(DataContext context) : base(context)
        {
        }
    }
}
