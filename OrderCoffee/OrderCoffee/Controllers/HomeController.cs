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

        private Dictionary<string, string> _dictIDCategoryToName = new Dictionary<string, string>();
        private Dictionary<string, string> _dictNameCategoryToID = new Dictionary<string, string>();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EditProduct()
        {
            ViewBag.Message = "Your application description page.";

            string querySelectProduct = "Select * from product";
            string querySelectCategory = "Select * from categories";

            using (IDbConnection db = conn)
            {
                var listProduct = new List<Product>();
                var listCategory = new List<Categories>();

                listProduct = db.Query<Product>(querySelectProduct).ToList();
                listCategory = db.Query<Categories>(querySelectCategory).ToList();

                ViewBag.listCategory = listCategory;
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
                    return View("EditProduct");
                    //EditProduct();
                }
            }
            return View("Index");
        }

        public JsonResult getInfoListProduct()
        {
            var jr = new JsonResult();

            string querySelectProduct = "Select * from product";

            var listProduct = new List<Product>();
            using (IDbConnection db = conn)
            {
                listProduct = db.Query<Product>(querySelectProduct).ToList();

                jr.Data = new
                {
                    list = listProduct,
                };
            }
            return Json(jr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult updateProduct(FormCollection collection)
        {
            _UpdateDictionaryCategory();

            var jr = new JsonResult();

            var Id = collection["Id"];
            var Name = collection["Name"];
            var Price = collection["Price"];
            var Description = collection["Description"];
            var categoryName = collection["CategoryName"];

            // ah dùng cái distionary đê lấy id_category nhe ah
            var id_category = _dictNameCategoryToID[categoryName];


            string queryUpdateProductt = "update product set id_category = '" + id_category + "', name = '" + Name + "', price = @Price, image = '" + Price + "', description = '" + Description + "', quantity = 0 where id = '" + Id + "'";

            string queryUpdateProduct = "UPDATE product SET id_category = @id_category,name = @Name,price = @Price,image =@link ,description =@Description,quantity=@Quantity where id = @Id; ";

            using (IDbConnection db = conn)
            {
                var affectedRows = db.Execute(queryUpdateProduct, new { id = Id, id_category = id_category, Name = Name, Price = Price, @link = "link", Description = Description, Quantity = 0 });
            }

            return Json(jr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult addProduct(FormCollection collection)
        {
            _UpdateDictionaryCategory();
            var jr = new JsonResult();

            var Name = collection["Name"];
            var Price = collection["Price"];
            var Description = collection["Description"];
            var categoryName = collection["CategoryName"];

            return Json(jr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult removeProduct(FormCollection collection)
        {
            var jr = new JsonResult(); 
            var id = collection["Id"]; 

            string queryRemoveProduct = "DELETE FROM product WHERE id = @idProduct";

            using(IDbConnection db = conn)
            {
                var affectedRows = db.Execute(queryRemoveProduct, new { idProduct = id });
            }

            return Json(jr, JsonRequestBehavior.AllowGet);
        }

        private void _UpdateDictionaryCategory()
        {
            if (_dictIDCategoryToName.Count == 0)
            {
                string querySelectCaterogy = "Select * from categories ";
                using (IDbConnection db = conn)
                {
                    var listCategory = new List<Categories>();
                    listCategory = db.Query<Categories>(querySelectCaterogy).ToList();
                    foreach (var item in listCategory)
                    {
                        if (!_dictIDCategoryToName.ContainsKey(item.Id))
                        {
                            _dictIDCategoryToName.Add(item.Id, item.Category_Name);
                            _dictNameCategoryToID.Add(item.Category_Name, item.Id);
                        }
                        else
                        {
                            _dictIDCategoryToName[item.Id] = item.Category_Name;
                            _dictNameCategoryToID[item.Category_Name] = item.Id;
                        }
                    }
                }
            }
        }
    }
}