using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTrackerWebAPI.Entities
{
    [Table(name: "users")]
    public class User
    {
        [Key]
        [Column(name: "user_id")]
        public Guid Id { get; set; }
        
        [EmailAddress]
        [Column(name: "email")]
        public string Email { get; set; }
        
        [Column(name: "nickname")]
        public string Nickname { get; set; }

        public UserCredentials UserCredentials { get; set; }
    }
}