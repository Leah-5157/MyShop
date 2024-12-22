using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService : IOrderService
    {
        IOrderRepository _OrderRepository;
        public OrderService(IOrderRepository OrderRepository)
        {
            _OrderRepository = OrderRepository;
        }

        public async Task<Order> GetByID(int id)
        {
            return await _OrderRepository.GetByID(id);
        }


        public async Task<Order> Post(Order order)
        {
            return await _OrderRepository.Post(order);
        }
    }
}
