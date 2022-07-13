using Northwind.Services.Blogging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Northwind.Services.Entities;
using System.IO;
using System.Text.Json;

namespace NorthwindApiApp.Controllers
{
    [Route("api/articles")]
    [ApiController]
    public class BlogArticlesController : ControllerBase
    {
        private readonly IBloggingService blogService;

        public BlogArticlesController(IBloggingService blogService)
        {
            this.blogService = blogService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogArticle(BlogArticle blogArticle)
        {
            if (blogArticle == null)
            {
                return this.BadRequest();
            }

            var id = await this.blogService.CreateBlogArticleAsync(blogArticle);

            return blogArticle is not null ? this.Ok($"New blog article created Id = {id}") : this.BadRequest();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBlogArticle(int id)
        {
            if (id <= 0)
            {
                return this.BadRequest();
            }

            var result = await this.blogService.DestroyBlogArticleAsync(id);

            return result ? this.Ok($"BlogArticle id={id} deleted.") : this.NotFound();
        }

        [HttpGet]
        public IActionResult ReadAllBlogArticles()
        {
            var result = this.blogService.GetBlogArticlesAsync().ToListAsync().Result;

            if (result is null)
            {
                return this.BadRequest();
            }

            if (!result.Any())
            {
                return this.NotFound();
            }

            var o = from r in result
                    select
                    new
                    {
                        r.blog.ArticleId,
                        r.blog.Title,
                        r.blog.PublishDate,
                        r.blog.EmployeeId,
                        authorName = r.employee.FirstName + " " +
                        r.employee.LastName + ", " + r.employee.Title,
                    };


            return this.Ok(o);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ReadBlogArticle(int id)
        {
            var (result, tuple) = await this.blogService.TryGetBlogArticleAsync(id);

            if (!result)
            {
                return this.NotFound();
            }

            return this.Ok(
                new
                {
                    tuple.blog.ArticleId,
                    tuple.blog.Title,
                    tuple.blog.PublishDate,
                    tuple.blog.EmployeeId,
                    authorName = tuple.employee.FirstName + " " + tuple.employee.LastName + ", " + tuple.employee.Title,
                    tuple.blog.Text,
                });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBlogArticle(int id, BlogArticle blog)
        {
            return await this.blogService.UpdateBlogArticleAsync(id, blog) ? this.Ok() : this.NotFound();
        }

        [HttpGet("{articleId:int}/products")]
        public ActionResult<IAsyncEnumerable<Product>> ReadRelatedProducts(int articleId)
        {
            var products = this.blogService.GetProductsForBlogArticleAsync(articleId);

            if (!products.AnyAsync().Result)
            {
                return this.NotFound();
            }

            return this.Ok(products);
        }

        [HttpPost("{articleId:int}/products/{productId:int}")]
        public async Task<IActionResult> PostBlogArticleProduct(int articleId, int productId)
        {
            var id = await this.blogService.CreateBlogArticleProductAsync(articleId, productId);

            if (id == -1)
            {
                return this.NotFound();
            }

            return this.Ok("New link created.");
        }

        [HttpDelete("{articleId:int}/products/{productId:int}")]
        public async Task<IActionResult> DeleteBlogArticleProduct(int articleId, int productId)
        {
            return await this.blogService.DestroyBlogArticleProductAsync(articleId, productId) ?
                this.Ok("Link was removed.") : this.NotFound();
        }

        [HttpPost("{articleId:int}/comments")]
        public async Task<IActionResult> PostBlogComment(int articleId)
        {
            using var stream = this.HttpContext.Request.Body;
            var options = new JsonSerializerOptions();

            var body = await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(stream, options);

            var text = body["text"].ToString();
            var customerId = int.Parse(body["customerId"].ToString());

            var result = await this.blogService.CreateCommentAsync(articleId, customerId, text);

            if (result == -1)
            {
                return this.NotFound();
            }

            return this.Ok("Comment was created.");
        }

        [HttpGet("{articleId:int}/comments")]
        public async Task<ActionResult<IAsyncEnumerable<BlogComment>>> ReadBlogCommentsByArticleId(int articleId)
        {
            var comments = this.blogService.ReadAllComments(articleId);

            if (!await comments.AnyAsync() || comments == null)
            {
                return this.NotFound();
            }

            return this.Ok(comments);
        }

        [HttpPut("{articleId:int}/comments/{customerId:int}")]
        public async Task<IActionResult> UpdateComment(int articleId, int customerId)
        {
            using var reader = new StreamReader(this.HttpContext.Request.Body);
            var text = await reader.ReadToEndAsync();

            var result = await this.blogService.UpdateCommentAsync(articleId, customerId, text);

            return result ? this.Ok("Comment had been changed.") : this.NotFound();
        }

        [HttpDelete("{articleId:int}/comments/{customerId:int}")]
        public async Task<IActionResult> DeleteComment(int articleId, int customerId)
        {
            var result = await this.blogService.DestroyCommentAsync(articleId, customerId);

            return result ? this.Ok("Comment had been deleted.") : this.NotFound();
        }
    }
}
