using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTracker.Core.Models
{
    [Table(name: "boards")]
    public class Board
    {
        [Key]
        [Column(name: "board_id")]
        public Guid Id { get; set; }
        
        [ForeignKey(nameof(User))]
        [Column(name: "owner_id")]
        public Guid OwnerId { get; set; }
        
        [Column(name: "name")]
        public string Name { get; set; }
        
        public User User { get; set; }
    }
}