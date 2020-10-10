using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderCoffee.Models
{
    public class Account
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string PassWord { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int Roles { get; set; }
    }
}