using System.ComponentModel.DataAnnotations;

namespace ChatApplication.Application.DTOs.Chat
{
    public class InitiateChatDTO
    {
        [Required(ErrorMessage ="Please enter Customer Name.")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage ="Please enter valid Customer Email.")]
        [Required(ErrorMessage ="Please enter Customer Email.")]
        public string Email { get; set; }
    }
}
