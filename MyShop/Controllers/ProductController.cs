using AutoMapper;
using DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
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
        public ProductController(IProductService ProductService, IMapper mapper)
        {
            _ProductService = ProductService;
            _Mapper = mapper;
        }
        // GET: api/<ProductController>
        [HttpGet]
        public async Task<IEnumerable<ProductDTO>> Get([FromQuery] string? desc, [FromQuery] int? minPrice, [FromQuery] int? maxPrice, [FromQuery] int?[] categoryIds)
        {
            
            IEnumerable<Product> products= await _ProductService.Get(desc,minPrice, maxPrice,categoryIds);
            IEnumerable<ProductDTO> productDTOs = _Mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);
            return productDTOs;
        }
    }
}
