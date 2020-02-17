using AutoMapper;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VER_CHK.Helpers;
using VER_CHK.Interfaces;
using VER_CHK.Interfaces.Articles;
using VER_CHK.Models.Articles;

namespace VER_CHK.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IMongoCollection<Article> _context;

        public ArticleService(IDatabaseSettings settings)
        {
            if(settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _context = database.GetCollection<Article>(settings.CollectionName) ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<List<Article>> GetAll() =>
            await (await _context.FindAsync(article => article.Title != null)).ToListAsync();

        public async Task<Article> Get(string articleName) =>
            await (await _context.FindAsync(article => article.Title == articleName)).FirstOrDefaultAsync();

        public async Task<List<Article>> GetArticle(string searchName)
        {
            var findArticle = await (await _context.FindAsync(article => article.Title != null)).ToListAsync();
            return findArticle.Where(t => t.Title.Contains(searchName) 
                || t.CreatedUser.Contains(searchName) || t.Category.Contains(searchName)).ToList();
        }

        public async Task<Article> Create(Article article)
        {
            if (await ((await _context.FindAsync(x => x.Title == article.Title)).FirstOrDefaultAsync()) != null)
                throw new AppException("Title \"" + article.Title + "\" is already taken");
            article.CreatedDate = DateTime.Now.ToString();

            await _context.InsertOneAsync(article);
            return article;
        }

        public async Task Update(Article article)
        {
            var currentArticle = await (await _context.FindAsync(x => x.Title == article.Title)).FirstOrDefaultAsync();

            if (currentArticle == null)
                throw new AppException("User not found");

            // update email if it has changed
            if (!string.IsNullOrWhiteSpace(article.Title))
            {
                // throw error if the new email is already taken
                if (await ((await _context.FindAsync(x => x.Title == article.Title)).FirstOrDefaultAsync()) != null)
                    throw new AppException("Title \"" + article.Title + "\" is already taken");

                currentArticle.Title = article.Title;
            }

            await _context.ReplaceOneAsync(article => article.Title == article.Title, currentArticle);
        }

        public async Task Delete(string articleName) =>
            await _context.DeleteOneAsync(article => article.Title == articleName);

        public async Task<Article> AddComment(string title, CommentModel commentArticle)
        {
            var currentArticle = await (await _context.FindAsync(article => article.Title == title)).FirstAsync();
            try
            {
                if (currentArticle.Comment == null)
                {
                    currentArticle.Comment = new List<CommentModel>();
                    currentArticle.Comment.Add(commentArticle);
                }
                else
                {
                    currentArticle.Comment.Add(commentArticle);
                }
                
            }catch(Exception ex)
            {
                throw new AppException("Error " + ex);
            }

            await _context.ReplaceOneAsync(article => article.Title == title, currentArticle);

            return currentArticle;
        }
    }
}
