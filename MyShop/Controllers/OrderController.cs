using AutoMapper;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using DTO;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        IOrderService _OrderService;
        IMapper _mapper;
        private readonly IMemoryCache _cache;

        public OrderController(IOrderService OrderService,IMapper mapper, IMemoryCache cache)
        {
            _OrderService = OrderService;
            _mapper = mapper;
            _cache = cache;
        }

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public async Task<OrderDTO> Get(int id)
        {
            if (!_cache.TryGetValue($"Order_{id}", out OrderDTO orderDTO))
            {
                Order order = await _OrderService.GetByID(id);
                orderDTO = _mapper.Map<Order, OrderDTO>(order);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)); 

                _cache.Set($"Order_{id}", orderDTO, cacheOptions);
            }
            return orderDTO;
        }

        // POST api/<OrderController>
        [HttpPost]
        public async Task<OrderDTO> Post([FromBody] PostOrderDTO order)
        {
            Order ord = await _OrderService.Post(_mapper.Map<PostOrderDTO, Order>(order));
            OrderDTO orderDTO = _mapper.Map<Order, OrderDTO>(ord);
            return orderDTO;
        }
    }
}
