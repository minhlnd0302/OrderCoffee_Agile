using Dapper;
using OrderCoffee.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

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

            string name;

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

        public ActionResult Login ()
        {
            return View();
        }

        public ActionResult Registration ()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string id_login_email, string id_login_password)
        {
            if (id_login_email != string.Empty && id_login_password != string.Empty)
            {
                string queryFindEmail = "Select * From dbo.Account Where Email = '" + id_login_email + "'";
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
                        ViewBag.nameLogin = user.UserName;
                        switch (user.Roles)
                        {
                            case 1:
                                // login of admin
                                return View("EditProduct");
                            case 2:
                                // login of user
                                return View("../Products/Index");
                            default:
                                // fail
                                break;
                        }
                    }
                }
            }

            ViewBag.error = "Login failed";
            // null
            return View();
        }

        [HttpPost]
        public ActionResult Registration(Account _accounts)
        {
            if (_accounts.UserName != null && _accounts.FullName != null && _accounts.Email != null && _accounts.PassWord != null && _accounts.PhoneNumber != null)
            {
                string queryInsert = "Insert Into account (username, name, password, number, email, roles) values (@username, @name, @password , @number, @email, @roles);";
                string queryFindEmail = "Select * From dbo.Account Where Email = '" + _accounts.Email + "'";
                string queryFindUserName = "Select * From dbo.Account Where UserName = '" + _accounts.UserName + "'";

                var user = new Account();
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ToString()))
                {
                    user = db.Query<Account>(queryFindUserName).FirstOrDefault();
                    if (user != null)
                    {
                        if (user.UserName == _accounts.UserName) return View();
                    }

                    user = db.Query<Account>(queryFindEmail).FirstOrDefault();
                    if (user != null)
                    {
                        if (user.Email == _accounts.Email) return View();
                    }

                    var affectedRows = db.Execute(queryInsert, new
                    {
                        username = _accounts.UserName,
                        name = _accounts.FullName,
                        password = _accounts.PassWord,
                        number = _accounts.PhoneNumber,
                        email = _accounts.Email,
                        roles = 2
                    });

                }
                return RedirectToAction("Index");
            }

            // call action register
            return View();
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
            var id_Category = _dictNameCategoryToID[categoryName]; 
             
            string queryUpdateProduct = "UPDATE product SET id_category = @id_category,name = @Name,price = @Price,image =@link ,description =@Description,quantity=@Quantity where id = @Id; ";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ToString()))
            {
                var affectedRows = db.Execute(queryUpdateProduct, new { id = Id, id_category = id_Category, Name = Name, Price = Price, link = "link", Description = Description, Quantity = 0 });
            }

            return Json(jr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult addProduct(FormCollection collection)
        {
            _UpdateDictionaryCategory();
            var jr = new JsonResult();

            string Id = "";
            var Name = collection["Name"];
            var Price = collection["Price"];
            var Description = collection["Description"];
            var categoryName = collection["CategoryName"];
            var categoryId = _dictNameCategoryToID[categoryName];

            string querySelectMaxId = "SELECT MAX(SUBSTRING(id, 6, len(id) - 6 + 1)) AS ExtractString FROM product;";

            using(IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ToString()))
            {
                string maxId = db.Query<string>(querySelectMaxId).First(); 
                var newId = int.Parse(maxId) + 1; 
                var tmp = "";
                if (newId < 10) tmp = "00" + newId.ToString();
                else if (newId < 100) tmp = "0" + newId.ToString(); 
                Id =  "PROD_" + tmp;

                string queryInsert = "Insert Into product (id, id_category, name, price, image, description, quantity) Values (@IdProduct, @IdCategory, @Name, @Price, @Image, @Description, @Quantity);";

                var affectedRows = db.Execute(queryInsert, new { IdProduct = Id, IdCategory = categoryId, Name = Name, Price = Price, Image = "link", Description = Description, Quantity = 0 });

                 
            }

            return Json(jr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult removeProduct(FormCollection collection)
        {
            var jr = new JsonResult(); 
            var id = collection["Id"]; 

            string queryRemoveProduct = "DELETE FROM product WHERE id = @idProduct";

            using(IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ToString()))
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