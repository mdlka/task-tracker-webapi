namespace TaskTrackerWebAPI.Entities
{
    public class TodoItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TodoItemState State { get; set; }
    }
    
    public class TodoItemSummaryDto
    {
        public string Name { get; set; }
    }
}