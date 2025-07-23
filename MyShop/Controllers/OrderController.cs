using AutoMapper;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using DTO;
using Microsoft.AspNetCore.Authorization;

namespace MyShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<OrderDTO> GetOrderById(int id)
        {
            var order = await _orderService.GetByID(id);
            return _mapper.Map<Order, OrderDTO>(order);
        }

        [HttpPost]
        public async Task<OrderDTO> CreateOrder([FromBody] PostOrderDTO postOrderDto)
        {
            var order = await _orderService.Post(_mapper.Map<PostOrderDTO, Order>(postOrderDto));
            return _mapper.Map<Order, OrderDTO>(order);
        }
    }
}
