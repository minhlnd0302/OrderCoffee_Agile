namespace OrderCoffee.Models
{
    public class Cart_Detail
    {
        public string ID_Cart { get; set; }
        public string ID_Product { get; set; }
        public int Quantity { get; set; }
        public int Unit_Price { get; set; }
    }
}