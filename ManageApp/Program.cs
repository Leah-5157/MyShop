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
            string connectionString = "data source=DESKTOP-E0FAPSB\\SQLEXPRESS;Database=LeahShopDB;Trusted_Connection=True;TrustServerCertificate=True;";
            Insert insert = new Insert();
            Read read = new Read();
            read.read(connectionString);

            insert.InsertCategory(connectionString);
            insert.InsertProduct(connectionString);
        }
    }
}
