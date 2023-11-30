using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Northwind.Services.Blogging;
using Northwind.Services.EntityFrameworkCore.Blogging.Context;

namespace Northwind.Services.EntityFrameworkCore.Blogging
{
    public class BloggingService : IBloggingService
    {
        private readonly BloggingContext context;

        public BloggingService(BloggingContext context)
        {
            this.context = context ?? throw new ArgumentNullException();
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

        public IAsyncEnumerable<BlogArticle> GetBlogArticlesAsync()
        {
            return this.context.BlogArticles.AsAsyncEnumerable();
        }

        public async Task<(bool result, BlogArticle blog)> TryGetBlogArticleAsync(int blogArticleId)
        {
            var blog = await this.context.BlogArticles.FindAsync(blogArticleId).ConfigureAwait(false);

            if (blog is null)
            {
                return (false, null);
            }

            return (true, blog);
        }

        public async Task<bool> UpdateBlogArticleAsync(int blogArticleId, BlogArticle blogArticle)
        {
            var blog = await this.context.BlogArticles.FindAsync(blogArticleId).ConfigureAwait(false);

            if (blog is not null)
            {
                blog.Title = blogArticle.Title;
                blog.Text = blogArticle.Text;
                blog.PublishDate = DateTime.Now;
                blog.EmployeeId = blogArticle.EmployeeId;
                await this.context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public IAsyncEnumerable<BlogArticleProduct> GetBlogArticleProductsAsync(int blogArticleId)
        {
            var blogArticleProducts = 
                this.context.BlogArticleProducts.AsAsyncEnumerable().
                Where(x => x.BlogArticleId == blogArticleId);

            return blogArticleProducts;
        }

        public async Task<bool> CreateBlogArticleProductAsync(int blogArticleId, int productId)
        {
            var articleProduct = new BlogArticleProduct 
            {
                BlogArticleId = blogArticleId,
                ProductId = productId,
            };

            try
            {
                if (!this.ProductLinkExists(blogArticleId, productId))
                {
                    await this.context.BlogArticleProducts.AddAsync(articleProduct);
                    await this.context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        public async Task<bool> DestroyBlogArticleProductAsync(int blogArticleId, int productId)
        {
            var blog = this.context.BlogArticleProducts.FirstOrDefault(
                x => x.BlogArticleId == blogArticleId && x.ProductId == productId);

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

            var comment = new BlogComment { BlogArticleId = blogArticleId, CustomerId = customerId, Comment = text };

            await this.context.BlogComments.AddAsync(comment);
            await this.context.SaveChangesAsync();

            return comment.BlogArticleId;
        }

        public IAsyncEnumerable<BlogComment> ReadAllComments(int blogArticleId)
        {
            return this.context.BlogComments.ToAsyncEnumerable().Where(x => x.BlogArticleId == blogArticleId);
        }

        public async Task<bool> UpdateCommentAsync(int blogArticleId, int customerId, string text)
        {
            var currentComment = this.context.BlogComments.FirstOrDefault(x =>
            x.BlogArticleId == blogArticleId && x.CustomerId == customerId);

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
            x.BlogArticleId == blogArticleId && x.CustomerId == customerId);

            if (currentComment is null)
            {
                return false;
            }

            this.context.BlogComments.Remove(currentComment);
            await this.context.SaveChangesAsync();

            return true;
        }

        private bool ProductLinkExists(int articleId, int productId)
        {
            return this.context.BlogArticleProducts.FirstOrDefault(
                x => x.BlogArticleId == articleId && x.ProductId == productId) is not null;
        }
    }
}
