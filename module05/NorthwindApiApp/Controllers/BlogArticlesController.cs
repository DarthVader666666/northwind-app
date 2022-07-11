using Northwind.Services.Blogging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Northwind.DataAccess;
using Northwind.DataAccess.Employees;
using Northwind.Services.Entities;
using Northwind.DataAccess.Products;

namespace NorthwindApiApp.Controllers
{
    [Route("api/articles")]
    [ApiController]
    public class BlogArticlesController : ControllerBase
    {
        private readonly IBloggingService blogService;
        private readonly IEmployeeDataAccessObject employeeService;
        private readonly IProductDataAccessObject productService;

        public BlogArticlesController(IBloggingService blogService, NorthwindDataAccessFactory factory)
        {
            this.blogService = blogService;
            this.employeeService = factory.GetEmployeeDataAccessObject();
            this.productService = factory.GetProductDataAccessObject();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogArticle(BlogArticle blogArticle)
        {
            if (blogArticle == null)
            {
                return this.BadRequest();
            }

            if (this.employeeService.FindEmployee(blogArticle.EmployeeId) is null)
            {
                return this.NotFound();
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
        public ActionResult<IAsyncEnumerable<object>> ReadAllBlogArticles()
        {
            var blogs = this.blogService.GetBlogArticlesAsync();

            if (blogs is null)
            {
                return this.BadRequest();
            }

            if (!blogs.AnyAsync().Result)
            {
                return this.NotFound();
            }

            var result = from b in blogs
                         join e in this.employeeService.SelectEmployees(0, 100) on b.EmployeeId equals e.EmployeeId
                         select new
                         {
                             b.ArticleId,
                             b.Title,
                             b.PublishDate,
                             b.EmployeeId,
                             authorName = e.FirstName + " " +
                             e.LastName + ", " + e.Title,
                         };

            return this.Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ReadBlogArticle(int id)
        {
            var (result, blog) = await this.blogService.TryGetBlogArticleAsync(id);

            if (!result)
            {
                return this.NotFound();
            }

            var employee = this.employeeService.FindEmployee(blog.EmployeeId);

            return this.Ok(new { blog.ArticleId, blog.Title, blog.PublishDate, blog.EmployeeId,
                authorName = employee.FirstName + " " + employee.LastName + ", " + employee.Title, blog.Text });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBlogArticle(int id, BlogArticle blog)
        {
            return await this.blogService.UpdateBlogArticleAsync(id, blog) ? this.Ok() : this.NotFound();
        }

        //[HttpGet("/{articleId:int}/products")]
        //public async Task<ActionResult<IAsyncEnumerable<Product>>> ReadRelatedProducts(int articleId)
        //{
        //    var (result, blogProduct) = await this.blogService.TryGetBlogArticleProductAsync(articleId);

        //    if (!result)
        //    {
        //        return this.NotFound();
        //    }

        //    var products = this.productService.SelectProducts()
        //}
    }
}
