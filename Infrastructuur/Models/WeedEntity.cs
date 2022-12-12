using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructuur.Models
{
    public enum TypeProduct
    {
        WEED,
        HASH,
        WEEDOIL,
        GRIT,
        LONGROLLINGPAPER
    }
    public class WeedEntity 
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string THC { get; set; }
        [Required]
        public string Flavours { get; set; }
        [Required]
        public string Effects { get; set; }
        [Required]
        public string Medicinal { get; set; }
        [Required]
        [Range(1,1000)]
        public double Price { get; set; }
        [Required]
        public string ImageFileLocation { get; set; }
        [Required]
        public string Info { get; set; }
        //public int Rating { get; set; }
        [Required]
        public TypeProduct TypeProduct { get; set; }
        public List<ReviewEntity>? Reviews { get; set; } = new List<ReviewEntity>();
        public ICollection<UserWeedEntity>? UserWeeds { get; set; }
    }

}
