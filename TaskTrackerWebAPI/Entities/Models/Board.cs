using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTrackerWebAPI.Entities
{
    [Table(name: "boards")]
    public class Board
    {
        [Key]
        [Column(name: "board_id")]
        public Guid Id { get; set; }
        
        [Column(name: "board_name")]
        public string Name { get; set; }
    }
}