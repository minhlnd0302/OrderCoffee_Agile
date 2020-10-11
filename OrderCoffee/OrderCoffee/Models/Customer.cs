namespace OrderCoffee.Models
{
    public class Customer
    {
        public string Id { get; set; }
        public string Name { set; get; }
        public string Password { set; get; }
        public int Phone { set; get; }
        public string Email { get; set; }
        public int Roles { get; set; }
    }
}