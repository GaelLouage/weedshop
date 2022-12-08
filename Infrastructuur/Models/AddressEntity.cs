using System.ComponentModel.DataAnnotations;

namespace Infrastructuur.Models
{
    
    public class AddressEntity
    {
        [Key]
        public int Id { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string AddressNumber { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<UserAddressEntity>? UserAddress { get; set; }
    }
}
