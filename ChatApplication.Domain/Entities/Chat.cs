using ChatApplication.Domain.Common;

namespace ChatApplication.Domain.Entities
{
    public class Chat : BaseEntity
    {
        public string ChatRequestId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedUTCDateTime { get; set; }
        public string SentBy { get; set; }
    }
}
