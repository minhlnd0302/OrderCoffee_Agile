using Dapper;
using OrderCoffee.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderCoffee.Controllers
{

    public class AccountController : Controller
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ToString());
        // GET: Accounts
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult updateProfile(string user_name, string full_name, string phone, string email, string password, string password2, string address)
        {
            var username = Session["userName"].ToString();

            string sql = "UPDATE Account SET name = @fullname, password = @pword, number = @numberphone, email = @mail, address = @ad WHERE username = @uname";

            try
            {
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ToString()))
                {
                    var affectedRows = db.Execute(sql, new { uname = username, fullname = full_name, pword = password, numberphone = phone, mail = email, ad = address });

                    Console.WriteLine(affectedRows);
                }
            }
            catch { };

           

            return View("Index");
        }



        // POST: Accounts/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public JsonResult GetPfofile()
        {
            JsonResult jr = new JsonResult();

            var username = Session["userName"].ToString();

            try
            {
                string query_Account = " select * from Account";
                var profileAccount = new Account();

                using (IDbConnection db = conn)
                {
                    List<Account> listAccount = db.Query<Account>(query_Account).ToList();
                    foreach (var item in listAccount)
                    {
                        if (username == item.UserName)
                        {
                            profileAccount = item;
                            break;
                        }
                    }
                }

                jr.Data = new
                {
                    status = "OK",
                    profile = profileAccount
                };

            }
            catch
            {
                jr.Data = new
                {
                    status = "F"
                };
            }
            return Json(jr, JsonRequestBehavior.AllowGet);
        }
    }
}
