using Entities;
using Microsoft.Extensions.Logging;
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
        IProductRepository _ProductRepository;
        ILogger<OrderService> _logger;
        public OrderService(IOrderRepository OrderRepository, IProductRepository productRepository, ILogger<OrderService> logger)
        {
            _OrderRepository = OrderRepository;
            _ProductRepository = productRepository;
            _logger = logger;
        }

        public async Task<Order> GetByID(int id)
        {
            return await _OrderRepository.GetByID(id);
        }


        public async Task<Order> Post(Order order)
        {     
            decimal amount = await CheckSum(order);
            if (amount != order.OrderSum) {
                _logger.LogCritical($"{order.UserId} tried to change the order sum");
                order.OrderSum = amount;
            }
            return await _OrderRepository.Post(order);
        }

        private async Task<decimal> CheckSum(Order order)
        {
            List<Product> products = await _ProductRepository.Get(null, null, null, []);

            decimal amount = 0;
            foreach (var item in order.OrderItems)
            {
                
                var product = products.Find(p => p.ProductId == item.ProductId);
                if (product != null) 
                {
                    amount += product.Price;
                }               
            }
            return amount;

        }
    }
}
