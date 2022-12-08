using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructuur.Models
{
    public class UserAddressEntity
    {
        [Key]
        public int UserAddressID { get; set; }
        public int UserId { get; set; }
        public UserEntity User { get; set; }
        public int AddressId { get; set; }
        public AddressEntity Address { get; set; }
    }
}
