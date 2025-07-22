using AutoMapper;
using DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Services;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        IProductService _ProductService;
        IMapper _Mapper;
        ILogger<ProductController> _logger;
        private readonly IDistributedCache _cache;
        public ProductController(IProductService ProductService, IMapper mapper, IDistributedCache cache, ILogger<ProductController> logger)
        {
            _ProductService = ProductService;
            _Mapper = mapper;
            _cache = cache;
            _logger = logger;
            _logger.LogInformation("Someone enter to the application");
        }
        // GET: api/<ProductController>
        [HttpGet]
        public async Task<IEnumerable<ProductDTO>> Get([FromQuery] string? desc, [FromQuery] int? minPrice, [FromQuery] int? maxPrice, [FromQuery] int?[] categoryIds)
        {
            string cacheKey = $"products_{desc}_{minPrice}_{maxPrice}_{string.Join(",", categoryIds)}";
            IEnumerable<ProductDTO>? productDTOs = null;
            var cached = await _cache.GetStringAsync(cacheKey);
            if (cached != null)
            {
                productDTOs = JsonSerializer.Deserialize<IEnumerable<ProductDTO>>(cached);
            }
            else
            {
                IEnumerable<Product> products = await _ProductService.Get(desc, minPrice, maxPrice, categoryIds);
                productDTOs = _Mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);
                var options = new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(10),
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                };
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(productDTOs), options);
            }
            return productDTOs ?? Enumerable.Empty<ProductDTO>();
        }
    }
}
