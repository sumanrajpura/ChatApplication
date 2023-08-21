using ChatApplication.Application.Repositories;
using ChatApplication.Domain.Common;
using ChatApplication.Domain.Entities;
using ChatApplication.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.Persistence.Repositories
{
    public class ChatRequestRepository : BaseRepository<ChatRequest>, IChatRequestRepository
    {
        protected readonly DataContext _context;
        public ChatRequestRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ChatRequest>> Get_All_Active_ChatRequests_Async()
        {
            return await _context.ChatRequests
                                 .Where(c => c.Status == (int)ChatRequestStatus.InQueue
                                             || c.Status == (int)ChatRequestStatus.Connected)
                                 .ToListAsync();
        }

        public async Task<List<ChatRequest>> Get_All_Connected_ChatRequests_Async()
        {
            return await _context.ChatRequests
                                 .Where(c => c.Status == (int)ChatRequestStatus.Connected)
                                 .ToListAsync();
        }


        public async Task<int> Get_Current_Queue_Length_Async()
        {
            return await _context.ChatRequests
                                    .Where(c => c.Status == (int)ChatRequestStatus.InQueue
                                                || c.Status == (int)ChatRequestStatus.Connected)
                                    .CountAsync();
        }

        public async Task<List<ChatRequest>> Get_InQueue_ChatRequests_Async()
        {
            return await _context.ChatRequests
                        .Where(c => c.Status == (int)ChatRequestStatus.InQueue)
                        .OrderBy(c => c.CreatedDateTime)
                        .ToListAsync();
        }
    }
}
