using Entities;
using Moq;
using System.Reflection.Metadata;
using Repositories;
using repositories;
using Moq.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TestProject2
{
    public class UserUnitTest
    {
        [Fact]
        public async Task GetUser_ValidCredentials_ReturnsUser()
        {
            //Arrange
            var user = new User { UserName = "Leah", Password = "12@34#eE" };
            var mockContext = new Mock<MyShopContext>();
            var users = new List<User>() { user };
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);
            var userRepository = new UserRepository(mockContext.Object);

            //Act
            var result = await userRepository.Login(user.UserName, user.Password);

            //Assert
            Assert.Equal(user, result);
        }
        [Fact]
        public async Task Post_ShouldAddUser()
        {
            // Arrange
            var user = new User { UserName = "Leah", Password = "12@34#eE" };

            var mockSet = new Mock<DbSet<User>>();
            var mockContext = new Mock<MyShopContext>();

            mockContext.Setup(m => m.Users).Returns(mockSet.Object);

            var userRepository = new UserRepository(mockContext.Object);

            // Act
            var result = await userRepository.Post(user);

            // Assert
            //mockSet.Verify(m => m.Add(It.IsAny<User>()), Times.Once);
            //mockContext.Verify(m => m.SaveChangesAsync(), Times.Once);
            Assert.Equal(user.UserName, result.UserName);
        }

    }

    }