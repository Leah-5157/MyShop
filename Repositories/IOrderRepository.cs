using Entities;

namespace Repositories
{
    public interface IOrderRepository
    {
        Task<Order> GetByID(int id);
        Task<Order> Post(Order order);
    }
}