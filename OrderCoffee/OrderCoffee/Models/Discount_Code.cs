using System;

namespace OrderCoffee.Models
{
    public class Discount_Code
    {
        public string Code { get; set; }

        public int Disc_Percent { get; set; }

        public DateTime Date_Start { get; set; }

        public DateTime Date_End { get; set; }
    }
}