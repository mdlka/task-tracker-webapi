using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace TaskTrackerWebAPI.Entities
{
    [Table(name: "users_credentials")]
    public class UserCredentials
    {
        [Key]
        [ForeignKey(nameof(User))]
        [Column(name: "user_id")]
        public Guid Id { get; set; }
        
        [EmailAddress]
        [Column(name: "login")]
        public string Login { get; set; }

        [Column(name: "password")]
        public string Password { get; set; }
        
        [Column(name: "version")]
        public int Version { get; set; }
        
        public User User { get; set; }
    }
}