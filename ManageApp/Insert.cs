using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Manager
{
    class Insert
    {
        public int InsertCategory(string connectionString)
        {
            int rowsAffected = 0;
            string categoryName;
            string toContinue = "y";
            while (toContinue == "y")
            {
                Console.WriteLine("Insert category name");
                categoryName = Console.ReadLine();
                Console.WriteLine("Continue?");
                toContinue = Console.ReadLine();

                string query = "INSERT INTO Categories(CategoryName) VALUES(@categoryName)";

                using (SqlConnection cn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.Add("@categoryName", SqlDbType.NVarChar, 50).Value = categoryName;
                    cn.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
            return rowsAffected;
        }

        public int InsertProduct(string connectionString)
        {
            int rowsAffected = 0;
            string toContinue = "y";
            while (toContinue == "y")
            {
                string categoryID, productName, description, price, imgUrl;
                Console.WriteLine("Insert category ID");
                categoryID = Console.ReadLine();
                Console.WriteLine("Insert product name");
                productName = Console.ReadLine();
                Console.WriteLine("Insert product description");
                description = Console.ReadLine();
                Console.WriteLine("Insert price");
                price = Console.ReadLine();
                Console.WriteLine("Insert image url");
                imgUrl = Console.ReadLine();
                Console.WriteLine("Continue?");
                toContinue = Console.ReadLine();

                string query = "INSERT INTO Products(CategoryID, ProductName, Description, Price, ImgURL) VALUES(@categoryID, @productName, @description, @price, @imgUrl)";

                using (SqlConnection cn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.Add("@categoryID", SqlDbType.Int).Value = int.Parse(categoryID);
                    cmd.Parameters.Add("@productName", SqlDbType.NVarChar, 50).Value = productName;
                    cmd.Parameters.Add("@description", SqlDbType.NVarChar, 50).Value = description;
                    cmd.Parameters.Add("@price", SqlDbType.Money).Value = decimal.Parse(price);
                    cmd.Parameters.Add("@imgUrl", SqlDbType.NVarChar, 100).Value = imgUrl;
                    cn.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
            return rowsAffected;
        }
    }

}



