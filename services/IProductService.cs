using Entities;

namespace Services
{
    public interface IProductService
    {
        Task<List<Product>> Get(string? desc, int? minPrice, int? maxPrice, int?[] categoryIds);
    }
}