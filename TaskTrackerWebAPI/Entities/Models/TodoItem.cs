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
        
        [ForeignKey(nameof(TodoBoard))]
        [Column(name: "board_id")]
        public Guid BoardId { get; set; }
        
        [Column(name: "item_name")]
        public string Name { get; set; }
        
        [EnumDataType(typeof(TodoItemState))]
        [Column(name: "item_state")]
        public TodoItemState State { get; set; }
        
        public TodoBoard TodoBoard { get; set; }
    }
}