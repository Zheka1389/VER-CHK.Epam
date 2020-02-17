using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VER_CHK.Controllers;
using VER_CHK.Interfaces.Users;
using VER_CHK.Models.Users;
using Xunit;

namespace XUnitTestProject
{
    public class UserUnitTest
    {
        private async Task<List<User>> GetAll()
        {
            var articles = new List<User>
            {
                new User() {Id = "1", UserName = "test"},

            };
            return articles;
        }

        [Fact]
        public void Ctor_ContextIsNull_ExceptionThrown()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new UsersController(null, null, null));       
            Assert.Equal(typeof(ArgumentNullException), exception.GetType());
        }

        [Fact]
        public void GetAllCountShouldReturnTwo()
        {

            var mock = new Mock<IUserService>();
            mock.Setup(x => x.GetAll()).Returns(GetAll());
            IUserService mongoService = mock.Object;

            var items = mongoService.GetAll();
            var count = items.Result.Count;

            Assert.Equal(1, count);
        }
    }
}
