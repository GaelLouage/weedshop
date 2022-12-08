using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructuur.Models
{
    public class UserWeedEntity
    {
        [Key]
        public int UserWeedId { get; set; }
      
        public int UserId { get; set; }
        public UserEntity User { get; set; }
        public int WeedId { get; set; }
        public WeedEntity Weed { get; set; }
    }
}
