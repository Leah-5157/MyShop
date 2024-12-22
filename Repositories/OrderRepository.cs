using Entities;
using Microsoft.EntityFrameworkCore;
using repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class OrderRepository : IOrderRepository
    {
        MyShopContext _myShopContext;
        public OrderRepository(MyShopContext myShopContext)
        {
            _myShopContext = myShopContext;
        }
        public async Task<Order> GetByID(int id)
        {
            return await _myShopContext.Orders.Include(u=>u.User).FirstOrDefaultAsync(order=>order.OrderId==id);
        }
        public async Task<Order> Post(Order order)
        {
            _myShopContext.Orders.Add(order);
            await _myShopContext.SaveChangesAsync();
            return order;
        }
    }
}
