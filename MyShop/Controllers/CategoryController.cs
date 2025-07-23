using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using DTO;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        ICategoryService _CategoryService;
        IMapper _mapper;

        public CategoryController(ICategoryService CategoryService, IMapper mapper)
        {
            _CategoryService = CategoryService;
            _mapper = mapper;
        }
        // GET: api/<CategoryController>
        [HttpGet]
        public async Task<List<CategoryDTO>> Get()
        {
            List<Category> categories = await _CategoryService.Get();
            List<CategoryDTO> categoriesDTO = _mapper.Map<List<Category>, List<CategoryDTO>>(categories);
            return categoriesDTO;
        }
    }
}
