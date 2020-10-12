using Dapper;
using OrderCoffee.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace OrderCoffee.Controllers
{
    public class HomeController : Controller
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ToString());
        public SqlConnection Conn
        {
            get
            {
                if (conn == null)
                {
                    conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ToString());
                }
                return conn;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EditProduct()
        {
            ViewBag.Message = "Your application description page.";

            string querySelectProduct = "Select * from product";

            
            using (IDbConnection db = conn)
            {
                var listProduct = new List<Product>();
                listProduct = db.Query<Product>(querySelectProduct).ToList();

                ViewBag.listProduct = listProduct;
            } 
            return View();
        }

        public ActionResult AdminDashboard()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Login(string id_login_email, string id_login_password)
        { 
            string queryFindEmail = "Select * From dbo.Account Where  Email = '" + id_login_email + "'";
            string queryFindUserName = "Select * From dbo.Account Where UserName = '" + id_login_email + "'";

            var user = new Account();
            using (IDbConnection db = conn)
            {
                user = db.Query<Account>(queryFindUserName).FirstOrDefault();

                if (user == null)
                {
                    user = db.Query<Account>(queryFindEmail).FirstOrDefault();
                }
            }

            if (user != null)
            {
                if (user.PassWord == id_login_password)
                {
                    return View("AdminDashboard");
                }
            } 
            return View("Index");
        }

        public JsonResult getInfoListProduct()
        {
            var jr = new JsonResult();

            string querySelectProduct = "Select * from product";

            var listProduct = new List<Product>();
            using(IDbConnection db = conn)
            {
                listProduct = db.Query<Product>(querySelectProduct).ToList();

                jr.Data = new
                {
                    list = listProduct,
                };
            } 
            return Json(jr, JsonRequestBehavior.AllowGet);
        }
    }
}