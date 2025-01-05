using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTrackerWebAPI.Entities
{
    [Table(name: "items")]
    public class TodoItem
    {
        [Key]
        [Column(name: "item_id")]
        public Guid Id { get; set; }
        
        [Column(name: "name")]
        public string Name { get; set; }
        
        [EnumDataType(typeof(TodoItemState))]
        [Column(name: "state")]
        public TodoItemState State { get; set; }
        
        [ForeignKey(nameof(Board))]
        [Column(name: "board_id")]
        public Guid BoardId { get; set; }
        
        public Board Board { get; set; }
    }
}