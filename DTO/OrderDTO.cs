using Entities;

namespace DTO
{
    public record OrderDTO(int OrderId, DateTime OrderDate, decimal OrderSum,string UserUserName);
    public record PostOrderDTO(DateTime OrderDate, decimal OrderSum, string UserUserName);
}