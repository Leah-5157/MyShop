using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp
{
    internal class Read
    {
        internal void read(string connectionString)
        {
            string queryString = "select * from Categories c join Products p on c.CategoryID = p.CategoryID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine("\t{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", reader[0], reader[1], reader[2], reader[3], reader[4], reader[5], reader[6]);

                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.Read();
            }
        }
    }
}

