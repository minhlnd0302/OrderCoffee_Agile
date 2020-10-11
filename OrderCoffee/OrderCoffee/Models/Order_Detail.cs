namespace OrderCoffee.Models
{
    public class Order_Detail
    {
        public string Id_Order { get; set; }
        public string Id_Product { get; set; }
        public int Quantity { get; set; }
        public int Unit_Price { get; set; }
        public int Total { get; set; }
    }
}