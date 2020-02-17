using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VER_CHK.Controllers;
using VER_CHK.Interfaces;
using VER_CHK.Interfaces.Articles;
using VER_CHK.Models.Articles;
using VER_CHK.Services;
using Xunit;

namespace XUnitTestProject
{
    public class ArticleUnitTest
    {
        private async Task<List<Article>> GetAll()
        {
            var articles = new List<Article>
            {
                new Article() {Id = "1", Title = "Test", Category = "first" , Content = " test", 
                    CreatedDate = "test", CreatedUser = "test"},
                new Article() {Id = "2", Title = "Test1", Category = "test1",
                    Content = " test1", CreatedDate = "test1", CreatedUser = "test1"}
            };
            return articles;
        }

        [Fact]
        public void GetAllCountShouldReturnTwo()
        {

            var mock = new Mock<IArticleService>();
            mock.Setup(x => x.GetAll()).Returns(GetAll());
            IArticleService mongoService = mock.Object;

            var items = mongoService.GetAll();
            var count = items.Result.Count;

            Assert.Equal(2, count);
        }

        [Fact]
        public void Article_EmptyCollection_Test()
        {

            var mock = new Mock<IArticleService>();
            mock.Setup(x => x.GetAll()).Returns(GetAll());
            var test = new ArticlesController(mock.Object);
            var items = test.GetAll().Result.Value.Count;
            Assert.Equal(2, items);
        }

        [Fact]
        public void Article_EmptyCollection_Test1()
        {

            var mock = new Mock<IArticleService>();
            mock.Setup(x => x.GetAll()).Returns(GetAll());
            var test = new ArticlesController(mock.Object);
            var items = test.Get("Test").Result.Value.Category;
            Assert.Equal("first", items);
        }

        //[Fact]
        //public async Task GetCategoryName()
        //{
        //    var newArticle = new Article()
        //    {
        //        Id = "3",
        //        Title = "Test2",
        //        Category = "first2",
        //        Content = " test2",
        //        CreatedDate = "test",
        //        CreatedUser = "test2"
        //    };
        //    var mock = new Mock<IArticleService>();
        //    mock.Setup(x => x.GetAll()).Returns(GetAll());

        //    var mongoService = mock.Object.Get("Test");

        //    var mongoService1 = mock.Object.Get("Test1");
        //    var mongoService2 = mock.Object.Get("Test2");
        //    //var count1 = mongoService.GetAll();
        //    //var tt = await mongoService.Create(newArticle);
        //    //var count = mongoService.GetAll().Result.Count;

        //    //Assert.Equal(3, count);
        //}


        [Fact]
        public void Ctor_ContextIsNull_Exception()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ArticlesController(null, null));

            Assert.Equal(typeof(ArgumentNullException), exception.GetType());
        }
    }
}

