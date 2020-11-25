namespace OrderCoffee.Models
{
    public class Customer
    {
        public long Id { get; set; }
        public string Name { set; get; }
        public string Password { set; get; }
        public string Email { get; set; }
        public int Roles { get; set; }
        public string address { get; set; }
        public string number { get; set; }
    }
}