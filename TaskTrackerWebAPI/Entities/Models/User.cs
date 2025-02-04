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
        
        [Column(name: "email")]
        public string Email { get; set; }
        
        [Column(name: "name")]
        public string Name { get; set; }
        
        [Column(name: "created_at", TypeName = "timestamptz")]
        public DateTime CreatedAt { get; set; }

        public UserCredentials UserCredentials { get; set; }
    }
}