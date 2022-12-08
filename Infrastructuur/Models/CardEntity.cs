using System.ComponentModel.DataAnnotations;

namespace Infrastructuur.Models
{
    public class CardEntity
    {
        [Key]
        public int Id { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public int UserId { get; set; }
        public UserEntity User { get; set; }    
    }

}
