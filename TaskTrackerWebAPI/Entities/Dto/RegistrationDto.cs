using System.ComponentModel.DataAnnotations;

namespace TaskTrackerWebAPI.Entities
{
    public class RegistrationDto
    {
        [EmailAddress]
        public string Email { get; set; }
        
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }
        
        [StringLength(50, MinimumLength = 4)]
        public string Name { get; set; }
    }
}