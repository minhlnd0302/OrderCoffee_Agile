using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrderCoffee.Models
{
    public class Account
    {

        public string UserName { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$")]
        public string PassWord { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 3)]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Email { get; set; }

        public int Roles { get; set; }
    }
}