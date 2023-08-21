using ChatApplication.Domain.Common;

namespace ChatApplication.Domain.Entities
{
    public class Agent : BaseEntity
    {
        public string Name { get; set; }
        public int MaxChatLoad { get; set; } = 10;
        public int CurrentChatLoad { get; set; }
        public double Seniority { get; set; }
        public bool IsOverFlowMember { get; set; }
        public int ShiftStartHour { get; set; }
        public int ShiftEndHour { get; set; }
    }
}
