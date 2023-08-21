using ChatApplication.Domain.Common;

namespace ChatApplication.Domain.Entities
{
    public class ChatRequest : BaseEntity
    {
        public Guid AgentId { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Guid CustomerId { get; set; }
        public int PollCounter { get; set; }
    }
}
