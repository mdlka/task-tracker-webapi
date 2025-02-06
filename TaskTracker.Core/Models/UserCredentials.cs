using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTracker.Core.Models
{
    [Table(name: "users_credentials")]
    public class UserCredentials
    {
        [Key]
        [ForeignKey(nameof(User))]
        [Column(name: "user_id")]
        public Guid UserId { get; set; }
        
        [Column(name: "login")]
        public string Login { get; set; }

        [Column(name: "password")]
        public string Password { get; set; }
        
        [Column(name: "version")]
        public int Version { get; set; }
        
        public User User { get; set; }
    }
}