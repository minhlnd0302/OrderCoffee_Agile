namespace OrderCoffee.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Id_Category { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Image { set; get; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }
}