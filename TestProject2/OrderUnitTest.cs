using Entities;
using Moq;
using Moq.EntityFrameworkCore;
using repositories;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class OrderUnitTest
    {
        [Fact]
        public async Task GetByID_ValidId_ReturnsOrder()
        {
            // Arrange
            var order = new Order
            {
                OrderId = 1,
                User = new User { Id = 1, UserName = "Leah" }
            };

            var orders = new List<Order> { order };

            var mockContext = new Mock<MyShopContext>(); // יצירת Mock ל-DbContext
            mockContext.Setup(x => x.Orders).ReturnsDbSet(orders); // החזרת DbSet מזויף

            var orderRepository = new OrderRepository(mockContext.Object);

            // Act
            var result = await orderRepository.GetByID(1);

            // Assert
            Assert.Equal(order, result);
        }

        [Fact]
        public async Task Post_ValidOrder_ReturnsAddedOrder()
        {
            // Arrange
            var order = new Order
            {
                OrderId = 1,
                User = new User { Id = 1, UserName = "Leah" }
            };

            var orders = new List<Order> { }; // רשימה ריקה המדמה את מסד הנתונים

            var mockContext = new Mock<MyShopContext>(); // יצירת Mock ל-DbContext
            mockContext.Setup(x => x.Orders).ReturnsDbSet(orders); // החזרת DbSet מזויף

            var orderRepository = new OrderRepository(mockContext.Object);

            // Act
            var result = await orderRepository.Post(order);

            // Assert
            Assert.Equal(order, result);
        }
    }
}
