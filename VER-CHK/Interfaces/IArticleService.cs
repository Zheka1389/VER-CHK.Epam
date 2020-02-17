using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VER_CHK.Models.Articles;

namespace VER_CHK.Interfaces.Articles
{
    public interface IArticleService
    {
        Task<List<Article>> GetAll();
        Task<Article> Get(string articleName);
        Task<List<Article>> GetArticle(string searchName);
        Task<Article> Create(Article article);
        Task Update(Article article);
        Task Delete(string articleName);
        Task<Article> AddComment(string title, CommentModel commentArticle);
    }
}
