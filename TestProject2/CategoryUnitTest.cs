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
    public class CategoryUnitTest
    {
        [Fact]
        public async Task Get_WhenCalled_ReturnsAllCategories()
        {
            // Arrange
            var categories = new List<Category>
    {
        new Category { CategoryId = 1, CategoryName = "Electronics" },
        new Category { CategoryId = 2, CategoryName = "Clothing" }
    };

            var mockContext = new Mock<MyShopContext>(); // יצירת Mock ל-DbContext
            mockContext.Setup(x => x.Categories).ReturnsDbSet(categories); // הגדרת DbSet מזויף

            var categoryRepository = new CategoryRepository(mockContext.Object);

            // Act
            var result = await categoryRepository.Get();

            // Assert
            Assert.Equal(categories, result);
        }
    }
}
