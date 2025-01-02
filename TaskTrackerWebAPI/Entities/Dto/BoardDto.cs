
namespace TaskTrackerWebAPI.Entities
{
    public class BoardDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class BoardSummaryDto
    {
        public string Name { get; set; }
    }
}