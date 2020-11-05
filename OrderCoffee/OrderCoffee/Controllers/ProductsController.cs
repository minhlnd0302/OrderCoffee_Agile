using OrderCoffee.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;

namespace OrderCoffee.Controllers
{
    public class ProductsController : Controller
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ToString());
        // GET: Products
        public ActionResult Index()
        {
            string querySelectProduct = "Select * from product";
            string name = ViewBag.nameLogin;
            string linkIndex = ViewBag.linkTitleLogin;

            using (IDbConnection db = conn)
            {
                var listProduct = new List<Product>(); 
                listProduct = db.Query<Product>(querySelectProduct).ToList();  
                ViewBag.listProduct = listProduct;
                ViewBag.count = listProduct.Count();

                if (name == null || name == "Sign In")
                {
                    ViewBag.nameLogin = "Sign In";
                    ViewBag.linkTitleLogin = "";
                }
            }
            return View();
        }

        public ActionResult viewCart()
        {
            
            return View();
        }
    }
}