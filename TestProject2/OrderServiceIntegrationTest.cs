using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using repositories;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests;

namespace TestProject
{
    public class OrderServiceIntegrationTest : IClassFixture<DatabaseFixure>
    {


        private readonly MyShopContext _context;
        private readonly OrderService _repository;

        public OrderServiceIntegrationTest(DatabaseFixure fixture)
        {
            _context = fixture.Context;
            var logger = NullLogger<OrderService>.Instance; 
            _repository = new OrderService(new OrderRepository(_context), new ProductRepository(_context),logger);
        }



        [Fact]
        public async Task Post_ShouldSaveOrder_WithCorrectTotalAmount()
        {
            //Arrange
            var category = new Category { CategoryName = "Electronics" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var product1 = new Product { ProductName = "Laptop", Price = 10, ImgUrl = "laptop.jpg", Category = category };
            var product2 = new Product { ProductName = "Phone", Price = 20, ImgUrl = "phone.jpg", Category = category };
            var product3 = new Product { ProductName = "Tablet", Price = 15, ImgUrl = "tablet.jpg", Category = category };

            _context.Products.AddRange(product1, product2, product3);
            await _context.SaveChangesAsync();

            var user = new User { UserName = "test@example.com", Password = "password123", FirstName = "John", LastName = "Doe" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                OrderSum = 0,  
                UserId = user.Id,
                OrderItems = new List<OrderItem>
            {
                new OrderItem { ProductId = product1.ProductId, Quantity = 1 },
                new OrderItem { ProductId = product2.ProductId, Quantity = 1 },
                new OrderItem { ProductId = product3.ProductId, Quantity = 1 }
            }
            };

            // Act: 
            var savedOrder = await _repository.Post(order);

            // Assert: 
            Assert.NotNull(savedOrder);
            Assert.Equal(45, savedOrder.OrderSum); 
            Assert.Equal(3, savedOrder.OrderItems.Count); 
            Assert.Equal(user.Id, savedOrder.UserId); 
        }
    }
}

