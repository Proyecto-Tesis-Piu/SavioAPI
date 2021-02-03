using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonetaAPI.Data;
using MonetaAPI.Models;

namespace MonetaAPI.Controllers
{
    [Route("api/blog")]
    public class BlogController : ControllerBase
    {
        private readonly BlogContext blogContext;

        public BlogController(BlogContext context)
        {
            blogContext = context;
        }

        // GET: api/blog
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogArticle>>> GetBlogArticles() {
            List<BlogArticle> articles = await blogContext.Articles.ToListAsync();
            foreach (BlogArticle article in articles) {
                article.Tags = article.TagsString.Split(',');
                article.Bibliography = article.BibliographyString.Split('|');
            }
            return articles;
        }

        // GET: api/blog/5
        [HttpGet("{stringId}")]
        public async Task<ActionResult<BlogArticle>> GetBlogArticle(string stringId)
        {
            Guid id;
            try
            {
                id = Guid.Parse(stringId);
            }
            catch
            {
                return NotFound();
            }

            BlogArticle article = await blogContext.Articles.FindAsync(id);

            if (article == null) 
            {
                return NotFound();
            }

            article.Tags = article.TagsString.Split(',');
            article.Bibliography = article.BibliographyString.Split('|');
            
            return article;
        }
    }
}
