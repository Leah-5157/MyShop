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
            string connectionString = "data source=srv2\\pupils;initial catalog=MyShopDB;Integrated Security=SSPI;Persist Security Info=False;TrustServerCertificate=true";
            Insert insert = new Insert();
            Read read = new Read();
            insert.InsertCategory(connectionString);
            insert.InsertProduct(connectionString);
            read.read(connectionString);
        }
    }
}
