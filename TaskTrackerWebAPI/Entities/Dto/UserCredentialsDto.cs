using System.ComponentModel.DataAnnotations;

namespace TaskTrackerWebAPI.Entities
{
    public class UserCredentialsDto
    {
        [EmailAddress]
        public string Email { get; set; }
        
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }
    }
}