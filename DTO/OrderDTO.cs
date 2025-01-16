using Entities;

namespace DTO
{
    public record OrderDTO(int OrderId, DateTime OrderDate, decimal OrderSum,string UserId);
    public record PostOrderDTO(DateTime OrderDate, decimal OrderSum, int UserId,List<OrderItemDTO> OrderItems);
}