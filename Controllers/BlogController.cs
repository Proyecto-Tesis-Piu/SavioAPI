using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
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
            List<BlogArticle> articles = await blogContext.Articles
                .OrderBy(item => item.DateValue)
                .ToListAsync();
            foreach (BlogArticle article in articles) {
                article.Tags = article.TagsString.Split(',');
                article.Date = article.DateValue.ToString("d \\de MMMM \\de yyyy", CultureInfo.CreateSpecificCulture("es-MX"));
            }
            List<BlogArticle> result = articles
                .Select(item => new BlogArticle() {
                    Date = item.Date,
                    Id = item.Id,
                    Title = item.Title,
                    Image = item.Image,
                    ShortText = item.ShortText,
                    Tags = item.Tags
                }).ToList();
            return result;
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
            if (article.BibliographyString != null) {
                article.Bibliography = new List<Bibliography>();
                foreach (String bibTemp in article.BibliographyString.Split('|')) {
                    String[] result = bibTemp.Split('^');
                    Bibliography bibliography = new Bibliography();
                    bibliography.Text = result[0];
                    if (result.Length > 1)
                    {
                        bibliography.Url = result[1];
                    }
                    article.Bibliography.Add(bibliography);
                }
            }
            article.Date = article.DateValue.ToString("d \\de MMMM \\de yyyy", CultureInfo.CreateSpecificCulture("es-MX"));

            return article;
        }
    }
}
