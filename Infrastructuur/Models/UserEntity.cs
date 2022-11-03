using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructuur.Models
{
    public class UserEntity
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public List<ReviewEntity>? Reviews { get; set; } = new List<ReviewEntity>();
        public int? CardEntityId { get; set; }
        public CardEntity? Card { get; set; }
        public ShoppingCartEntity? ShoppingCart{ get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public List<AddressEntity> Addresses { get; set; } = new List<AddressEntity>();

        public List<WeedEntity>? Weeds { get; set; } = new List<WeedEntity>();
    }
}
