using AutoMapper;
using DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Services;

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
        private readonly IMemoryCache _cache;
        public ProductController(IProductService ProductService, IMapper mapper, IMemoryCache cache, ILogger<ProductController> logger)
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

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<ProductDTO> productDTOs))
            {
                IEnumerable<Product> products = await _ProductService.Get(desc, minPrice, maxPrice, categoryIds);
                productDTOs = _Mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10)) 
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1)); 

                _cache.Set(cacheKey, productDTOs, cacheEntryOptions);
            }

            return productDTOs;
        }
    }
}
