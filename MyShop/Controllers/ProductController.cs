using AutoMapper;
using DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Services;
using System.Text.Json;

namespace MyShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;
        private readonly IDistributedCache _cache;

        public ProductController(IProductService productService, IMapper mapper, IDistributedCache cache, ILogger<ProductController> logger)
        {
            _productService = productService;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductDTO>> GetProducts([FromQuery] string? desc, [FromQuery] int? minPrice, [FromQuery] int? maxPrice, [FromQuery] int?[] categoryIds)
        {
            string cacheKey = $"products_{desc}_{minPrice}_{maxPrice}_{string.Join(",", categoryIds)}";
            IEnumerable<ProductDTO>? productDTOs = null;
            var cached = await _cache.GetStringAsync(cacheKey);
            if (cached != null)
            {
                productDTOs = JsonSerializer.Deserialize<IEnumerable<ProductDTO>>(cached);
            }
            if (productDTOs == null)
            {
                var products = await _productService.Get(desc, minPrice, maxPrice, categoryIds);
                productDTOs = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);
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
