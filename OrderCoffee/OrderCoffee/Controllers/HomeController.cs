using Dapper;
using OrderCoffee.Models;
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

                if (user == null) {
                    user = db.Query<Account>(queryFindEmail).FirstOrDefault();
                }
            }

            if (user != null)
            {
                if(user.PassWord == id_login_password)
                {
                    return View("AdminDashboard");
                }
            }
            var tmp = id_login_email;
            return View("Index");
        }
    }
}