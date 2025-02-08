using System.ComponentModel.DataAnnotations;

namespace TaskTracker.WebAPI.Dto
{
    public class CreateBoardRequest
    {
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }
    }
}