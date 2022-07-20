using System;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.Blogging;

namespace Northwind.Services.EntityFrameworkCore.Blogging.Context
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

        public virtual DbSet<BlogComment> BlogComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogArticleProduct>(entity =>
            {
                entity.HasKey(e => new { e.BlogArticleId, e.ProductId });
            });

            modelBuilder.Entity<BlogComment>(entity =>
            {
                entity.HasKey(e => new { e.BlogArticleId, e.CustomerId });
            });
        }
    }
}
