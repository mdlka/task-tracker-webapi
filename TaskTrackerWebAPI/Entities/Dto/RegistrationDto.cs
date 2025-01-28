using System.ComponentModel.DataAnnotations;

namespace TaskTrackerWebAPI.Entities
{
    public class RegistrationDto
    {
        [EmailAddress]
        public string Email { get; set; }
        
        [MinLength(6)]
        public string Password { get; set; }
        
        [MinLength(4)]
        public string Name { get; set; }
    }
}