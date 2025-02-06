using System.ComponentModel.DataAnnotations;

namespace TaskTracker.WebAPI.Dto
{
    public class BoardRequest
    {
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }
    }
}