using AutoMapper;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        IOrderService _OrderService;
        IMapper _mapper;
        public OrderController(IOrderService OrderService,IMapper mapper)
        {
            _OrderService = OrderService;
            _mapper = mapper;
        }

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public async Task<OrderDTO> Get(int id)
        {
            // return await _OrderService.GetByID(id);
            Order order = await _OrderService.GetByID(id);
            OrderDTO orderDTO = _mapper.Map<Order, OrderDTO>(order);
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
