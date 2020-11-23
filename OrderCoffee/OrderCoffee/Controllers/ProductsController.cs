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
using Newtonsoft.Json;
using System.Dynamic;
using OrderCoffee.Common;

namespace OrderCoffee.Controllers
{
    public class ProductsController : Controller
    {
       public string code { get; set; }

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
       
        
        [HttpPost]
        public JsonResult CheckOut(string p,string tmp_total,string discountcode)
        {
            var id_cus= Session[CommonSession.ACCOUNT_SESSION].ToString();
            if (id_cus != null)
            {
                int percent;
                if (discountcode != "")
                {
                    Discount_Code code = conn.Query<Discount_Code>("Select * from discount_code where code = '" + discountcode+"'").FirstOrDefault();
                     percent= code.Disc_Percent;
                }
                else
                {
                    percent = 0;
                }
                // lấy thông tin khách hàng
                Customer customer = conn.Query<Customer>("Select * from account where id_customer =" + Convert.ToInt64(id_cus)).FirstOrDefault();

                dynamic json = JsonConvert.DeserializeObject(p);// lấy giỏ hàng từ local
                
                string queryString = "INSERT INTO dbo.[order] (id,id_customer,temp_total,discount,total,status) values (@id,@id_customer,@temp_total,@discount,@total,@status);";
                // insert vào table order trước mới insert vào order_detail theo id_order
                using (IDbConnection db = conn)
                {
                    var list = conn.Query<Order>("Select * FROM dbo.[order]").ToList();
                    string id_order = "Oder_" + (list.Count()+1).ToString();
                    // total thì tính dựa theo discount với temp_total
                    int totalReal = Convert.ToInt32(tmp_total) - (Convert.ToInt32(tmp_total) / 100 * percent);

                    conn.Execute(queryString, new { id = id_order, id_customer = Convert.ToInt64(id_cus),temp_total = Convert.ToInt32(tmp_total) , discount = percent, total = totalReal, status=0 });
                    string query = "Insert into dbo.[order_detail] (id_order,id_product,quantity ,unit_price,total) values (@id_order,@id_product,@quantity,@unit_price,@total);";

                    foreach (var item in json)
                    {
                        string id_ordera = id_order;
                        string ID = item["Id"];
                        int quan = item["Quantity"];
                        int price = item["Price"];
                        int tong = quan * price;
                        conn.Execute(query, new { id_order = id_ordera, id_product = ID, quantity = quan, unit_price = price, total = tong });
                        
                    }
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                // viết thong bao can phai dang nhap mới checkout được
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}