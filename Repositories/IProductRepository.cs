using Entities;

namespace Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> Get(string? desc, int? minPrice, int? maxPrice, int?[] categoryIds);
    }
}