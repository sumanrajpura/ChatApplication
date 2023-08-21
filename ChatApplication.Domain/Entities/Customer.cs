using ChatApplication.Domain.Common;

namespace ChatApplication.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
