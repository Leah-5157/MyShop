using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;
using repositories;

namespace Repositories
{
    public class ProductRepository : IProductRepository
    {
        MyShopContext _myShopContext;
        public ProductRepository(MyShopContext myShopContext)
        {
            _myShopContext = myShopContext;
        }

        public async Task<List<Product>> Get(string? desc,int? minPrice,int? maxPrice, int?[] categoryIds)
        {
            var query = _myShopContext.Products.Where(Product =>
            (desc == null ? (true) : (Product.Description.Contains(desc)))
                  &&(minPrice == null ? (true) : (Product.Price >= minPrice))
                  && ((maxPrice == null) ? (true) : (Product.Price <= maxPrice))
                  && ((categoryIds.Length == 0) ? (true) : (categoryIds.Contains(Product.CategoryId))))
                 .OrderBy(Product => Product.Price);
            Console.WriteLine(query.ToQueryString());
            List<Product> products = await query.Include(c=>c.Category).ToListAsync();
            return products;
        }
    }
}
