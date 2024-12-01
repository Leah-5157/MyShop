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

                string query = "INSERT INTO Categories(Category)" +
                          "VALUES(@categoryName)";

                using (SqlConnection cn = new SqlConnection(connectionString))

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.Add("@categoryName", SqlDbType.VarChar).Value = categoryName;

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

                string category_ID, productName, productDescription, price, imagePath;
                Console.WriteLine("Insert category ID");
                category_ID = Console.ReadLine();
                Console.WriteLine("Insert product name");
                productName = Console.ReadLine();
                Console.WriteLine("Insert product descriptio");
                productDescription = Console.ReadLine();
                Console.WriteLine("Insert price");
                price = Console.ReadLine();
                Console.WriteLine("Insert image path");
                imagePath = Console.ReadLine();
                Console.WriteLine("Continue?");
                toContinue = Console.ReadLine();

                string query = "INSERT INTO Products(Category_ID, ProductName,ProductDescription, Price, ImgePath)" +
                              "VALUES(@category_ID, @productName, @productDescription, @price, @imagePath)";

                using (SqlConnection cn = new SqlConnection(connectionString))

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.Add("@category_ID", SqlDbType.VarChar).Value = category_ID;
                    cmd.Parameters.Add("@productName", SqlDbType.VarChar).Value = productName;
                    cmd.Parameters.Add("@productDescription", SqlDbType.VarChar).Value = productDescription;
                    cmd.Parameters.Add("@price", SqlDbType.VarChar).Value = price;
                    cmd.Parameters.Add("@imagePath", SqlDbType.VarChar).Value = imagePath;

                    cn.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
            return rowsAffected;

        }
    }

}



