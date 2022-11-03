namespace Infrastructuur.Models
{
    public class ShoppingCartEntity 
    {
        public string Image { get; set; }
        public string Product { get; set; }
        public double Price { get; set; }   
        public int NumberOf { get; set; }
        public double SubTotal { get; set; }
        public double Total { get; set; }


        public double GetSubTotalOnItem() => Price * NumberOf;
    }

}
