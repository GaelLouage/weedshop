using System.ComponentModel.DataAnnotations;

namespace Infrastructuur.Models
{
    public class ReviewEntity
    {
        public int Id { get; set; }
        [Required]
        public string User { get; set; }
        [Required]
        public string ReviewText { get; set; }
        [Required]
        public DateTime ReviewDate { get; set; }
        [Required]
        public int Rating { get; set; }
    }
}