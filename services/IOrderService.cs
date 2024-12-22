using Entities;

namespace Services
{
    public interface IOrderService
    {
        Task<Order> GetByID(int id);
        Task<Order> Post(Order order);
    }
}