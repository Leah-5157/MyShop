using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // connection string for 'home'
            string connectionString = "Server=SRV2\\PUPILS;Database=My_Shop;Trusted_Connection=True;TrustServerCertificate=True";
            Insert insert = new Insert();
            Read read = new Read();
            read.read(connectionString);

            insert.InsertCategory(connectionString);
            insert.InsertProduct(connectionString);
        }
    }
}
