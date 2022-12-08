using System.ComponentModel.DataAnnotations;

namespace Infrastructuur.Models
{
    public class ReviewEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public int UserId { get; set; }
        public UserEntity User { get; set; }
        [Required]
        public int WeedId { get; set; }
        public WeedEntity Weed { get; set; }
        [Required]
        public string ReviewText { get; set; }
        [Required]
        public DateTime ReviewDate { get; set; }
        [Required]
        public int Rating { get; set; }
       
    }
}