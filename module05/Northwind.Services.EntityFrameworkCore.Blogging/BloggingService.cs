using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Northwind.Services.Blogging;
using Northwind.Services.EntityFrameworkCore.Blogging.Context;

namespace Northwind.Services.EntityFrameworkCore.Blogging
{
    public class BloggingService : IBloggingService
    {
        private readonly BloggingContext context;

        public BloggingService(DesignTimeBloggingContextFactory factory)
        {
            this.context = factory.CreateDbContext(null);
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

        public async Task<(bool result, BlogArticle blogArticle)> TryGetBlogArticleAsync(int blogArticleId)
        {
            var blog = await this.context.BlogArticles.FindAsync(blogArticleId).ConfigureAwait(false);

            return (blog is not null, blog);
        }

        public async Task<(bool result, BlogArticleProduct blogArticle)> TryGetBlogArticleProductAsync(int blogArticleId)
        {
            var blogProduct = await this.context.BlogArticleProducts.FindAsync(blogArticleId).ConfigureAwait(false);

            return (blogProduct is not null, blogProduct);
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
    }
}
