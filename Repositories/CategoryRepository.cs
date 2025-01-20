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
    public class CategoryRepository : ICategoryRepository
    {
        MyShopContext _myShopContext;
        public CategoryRepository(MyShopContext myShopContext)
        {
            _myShopContext = myShopContext;
        }
        public async Task<List<Category>> Get()
        {
            return await _myShopContext.Categories.ToListAsync();
            }
    }
}
