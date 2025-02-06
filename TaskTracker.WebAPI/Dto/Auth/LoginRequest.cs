using System.ComponentModel.DataAnnotations;

namespace TaskTracker.WebAPI.Dto
{
    public class LoginRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }
    }
}