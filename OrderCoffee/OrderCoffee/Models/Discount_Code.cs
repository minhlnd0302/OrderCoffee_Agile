using System;

namespace OrderCoffee.Models
{
    public class Discount_Code
    {
        public string Code { get; set; }

        public int Disc_Percent { get; set; }

        public string Date_Start { get; set; }

        public string Date_End { get; set; }
    }
}