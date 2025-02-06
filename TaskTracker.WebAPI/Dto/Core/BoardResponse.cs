using System.ComponentModel.DataAnnotations;

namespace TaskTracker.WebAPI.Dto
{
    public class BoardResponse
    {
        public Guid Id { get; set; }
        
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }
    }
}