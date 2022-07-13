using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Northwind.Services.Blogging;
using Northwind.Services.Entities;
using Northwind.Services.EntityFrameworkCore.Blogging.Context;
using Northwind.Services.Employees;

namespace Northwind.Services.EntityFrameworkCore.Blogging
{
    public class BloggingService : IBloggingService
    {
        private readonly BloggingContext context;
        private readonly IEmployeeManagementService employeeService;
        private readonly IProductManagementService productService;

        public BloggingService(DesignTimeBloggingContextFactory blogFactory,
            IEmployeeManagementService employeeService,
            IProductManagementService productService)
        {
            this.context = blogFactory.CreateDbContext(null);
            this.employeeService = employeeService;
            this.productService = productService;
        }

        public async Task<int> CreateBlogArticleAsync(BlogArticle blogArticle)
        {
            blogArticle.PublishDate = DateTime.Now;
            await this.context.BlogArticles.AddAsync(blogArticle);
            await this.context.SaveChangesAsync();
            return blogArticle.ArticleId;
        }

        public async Task<bool> DestroyBlogArticleAsync(int blogArticleId)
        {
            var blogArticle = await this.context.BlogArticles.FindAsync(blogArticleId);

            if (blogArticle is null)
            {
                return false;
            }

            this.context.BlogArticles.Remove(blogArticle);
            await this.context.SaveChangesAsync();

            return true;
        }

        public IAsyncEnumerable<(BlogArticle blog, Employee employee)> GetBlogArticlesAsync()
        {
            var blogs = this.context.BlogArticles.ToList();
            var employees = this.employeeService.GetEmployeesAsync(0, 100).ToListAsync().Result;

            var result = from b in blogs
                         from e in employees
                         where b.EmployeeId == e.EmployeeId
                         select (b, e);

            return result.ToAsyncEnumerable();
        }

        public async Task<(bool result, (BlogArticle blog, Employee employee))> TryGetBlogArticleAsync(int blogArticleId)
        {
            var blog = await this.context.BlogArticles.FindAsync(blogArticleId).ConfigureAwait(false);
            var employee = this.employeeService.TryGetEmployeeAsync(blog.EmployeeId).Result.employee;

            if (blog is null || employee is null)
            {
                return (false, (null, null));
            }

            return (true, (blog, employee));
        }

        public IAsyncEnumerable<Product> GetProductsForBlogArticleAsync(int blogArticleId)
        {
            var blogArticleProducts = this.context.BlogArticleProducts.ToList();

            return blogArticleProducts.Where(x => x.ArticleId == blogArticleId).
                Select(x => this.productService.TryGetProductAsync(x.ProductId).Result.product).ToAsyncEnumerable();
        }

        public async Task<bool> UpdateBlogArticleAsync(int blogArticleId, BlogArticle blogArticle)
        {
            var blog = await this.context.BlogArticles.FindAsync(blogArticleId).ConfigureAwait(false);

            if (blog is not null)
            {
                blog.Title = blogArticle.Title;
                blog.Text = blogArticle.Text;
                blog.PublishDate = DateTime.Now;
                await this.context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<int> CreateBlogArticleProductAsync(int blogArticle, int productId)
        {
            var productResult = await this.productService.TryGetProductAsync(productId);
            var blogResult = await TryGetBlogArticleAsync(blogArticle);

            if (!(productResult.result && blogResult.result))
            {
                return -1;
            }

            var blogArticleProducts =
                new BlogArticleProduct
                {
                    ProductId = productResult.product.ProductId,
                    ArticleId = blogResult.Item2.blog.ArticleId,
                };

            await this.context.BlogArticleProducts.AddAsync(blogArticleProducts);
            await this.context.SaveChangesAsync();

            return blogArticleProducts.ArticleProductId;
        }

        public async Task<bool> DestroyBlogArticleProductAsync(int blogArticle, int productId)
        {
            var blog = this.context.BlogArticleProducts.FirstOrDefault(
                x => x.ArticleId == blogArticle && x.ProductId == productId);

            if (blog == null)
            {
                return false;
            }

            this.context.BlogArticleProducts.Remove(blog);
            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<int> CreateCommentAsync(int blogArticleId, int customerId, string text)
        {
            var blog = await this.context.BlogArticles.FindAsync(blogArticleId);

            if (blog is null)
            {
                return -1;
            }

            var comment = new BlogComment { ArticleId = blogArticleId, CustomerId = customerId, Comment = text };

            await this.context.BlogComments.AddAsync(comment);
            await this.context.SaveChangesAsync();

            return comment.BlogCommentId;
        }

        public IAsyncEnumerable<BlogComment> ReadAllComments(int blogArticleId)
        {
            return this.context.BlogComments.Where(x => x.ArticleId == blogArticleId).ToAsyncEnumerable();
        }

        public async Task<bool> UpdateCommentAsync(int blogArticleId, int customerId, string text)
        {
            var currentComment = this.context.BlogComments.FirstOrDefault(x =>
            x.ArticleId == blogArticleId && x.CustomerId == customerId);

            if (currentComment is null)
            {
                return false;
            }

            currentComment.Comment = text;
            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DestroyCommentAsync(int blogArticleId, int customerId)
        {
            var currentComment = this.context.BlogComments.FirstOrDefault(x =>
            x.ArticleId == blogArticleId && x.CustomerId == customerId);

            if (currentComment is null)
            {
                return false;
            }

            this.context.BlogComments.Remove(currentComment);
            await this.context.SaveChangesAsync();

            return true;
        }
    }
}
