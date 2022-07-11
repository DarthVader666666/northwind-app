using System;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.Blogging;

namespace Northwind.Services.EntityFrameworkCore.Blogging
{
    public class BloggingContext : DbContext
    {
        public BloggingContext()
        {
        }

        public BloggingContext(DbContextOptions<BloggingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BlogArticle> BlogArticles { get; set; }

        public virtual DbSet<BlogArticleProduct> BlogArticleProducts { get; set; }
    }
}
