using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VER_CHK.Helpers;
using VER_CHK.Interfaces.Articles;
using VER_CHK.Models.Articles;
using VER_CHK.Services;

namespace VER_CHK.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ArticlesController : ControllerBase
    {
        private IArticleService _articleService;
        private readonly IMapper _mapper;

        public ArticlesController(ArticleService articleService, IMapper mapper)
        {
            _articleService = articleService ?? throw new ArgumentNullException(nameof(articleService));
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<ActionResult<Article>> Create([FromBody]CreateModel articleCreate)
        {
            var currentArticle = _mapper.Map<Article>(articleCreate);
            try
            {
                return Ok(await _articleService.Create(currentArticle));
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("title")]
        public async Task<IActionResult> AddComment([FromBody]CommentModel commentArticle)
        {
            commentArticle.CreatedDate = DateTime.Now.ToString();
            
            return Ok(await _articleService.AddComment(commentArticle.Title, commentArticle));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<Article>>> GetAll() =>
            await _articleService.GetAll();

        [AllowAnonymous]
        [HttpGet("{articleName}", Name = "GetArticle")]
        public async Task<ActionResult<Article>> Get(string articleName)
        {
            var article = await _articleService.Get(articleName);
            if (article == null)
                return BadRequest(new { message = "Article is incorrect or not in base" });

            return article;
        }

        [AllowAnonymous]
        [HttpGet("search/{searchName}", Name = "GetTitle")]
        public async Task<ActionResult<List<Article>>> GetArticle(string searchName)
        {
            var article = await _articleService.GetArticle(searchName);
            if (article == null)
                return BadRequest(new { message = "Article is incorrect or not in base" });

            return article;
        }

        [HttpDelete("{articleName}")]
        public async Task<IActionResult> Delete(string articleName)
        {
            var article = await _articleService.Get(articleName);

            if (article == null)
            {
                return BadRequest(new { message = "Article not found" });
            }

            await _articleService.Delete(articleName);

            return Ok();
        }
    }
}
