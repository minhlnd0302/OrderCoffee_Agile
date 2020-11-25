using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Schema;

namespace OrderCoffee.Models
{
    public class Order
    {
        public string Id { get; set; }
        public long Id_Customer { get; set; }
        public int Temp_Total { get; set; }
        public int Discount { get; set; }
        public int Total { get; set; }
        public int Status { get; set; }
    }
}