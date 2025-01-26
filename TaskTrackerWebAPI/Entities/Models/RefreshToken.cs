using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTrackerWebAPI.Entities
{
    [Table(name: "refresh_tokens")]
    public class RefreshToken
    {
        [Key]
        [ForeignKey(nameof(User))]
        [Column(name: "user_id")]
        public Guid Id { get; set; }
        
        [Column(name: "refresh_token")]
        public string Token { get; set; }
        
        [Column(name: "expires_at", TypeName = "timestamptz")]
        public DateTime ExpiresAt { get; set; }
        
        public User User { get; set; }
    }
}