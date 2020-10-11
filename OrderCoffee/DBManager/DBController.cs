using System;

namespace DatabaseProject
{
    public class DBController
    {
        private static DBController instance = null;

        public static DBController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DBController();
                }
                return instance;
            }
        }


        //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ToString());

        private DBController()
        {

        }
    }
}
