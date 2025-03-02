using Microsoft.EntityFrameworkCore;
using repositories;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class DatabaseFixure
    {
        public MyShopContext Context { get; private set; }
        public DatabaseFixure()
        {
            var options = new DbContextOptionsBuilder<MyShopContext>()
                .UseSqlServer("Server = SRV2\\PUPILS; Database = My_ShopTest; Trusted_Connection = True; TrustServerCertificate = True")
                .Options;
            Context = new MyShopContext(options);
            Context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            Context.Database.EnsureCreated();
            Context.Dispose();
        }
    }
}
