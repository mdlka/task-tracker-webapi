using System.ComponentModel.DataAnnotations;

namespace TaskTrackerWebAPI.Entities
{
    public class BoardDto
    {
        public Guid Id { get; set; }
        
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }
    }

    public class BoardSummaryDto
    {
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }
    }
}