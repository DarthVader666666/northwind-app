﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Northwind.Services.EntityFrameworkCore.Blogging.Context;

namespace Northwind.Services.EntityFrameworkCore.Blogging.Migrations
{
    [DbContext(typeof(BloggingContext))]
    [Migration("20220720173758_AddBlogComment")]
    partial class AddBlogComment
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Northwind.Services.Blogging.BlogArticle", b =>
                {
                    b.Property<int>("ArticleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("article_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int")
                        .HasColumnName("employee_id");

                    b.Property<DateTime>("PublishDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("publish_date");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)")
                        .HasColumnName("title");

                    b.HasKey("ArticleId");

                    b.ToTable("BlogArticles");
                });

            modelBuilder.Entity("Northwind.Services.Blogging.BlogArticleProduct", b =>
                {
                    b.Property<int>("BlogArticleId")
                        .HasColumnType("int")
                        .HasColumnName("article_id");

                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("product_id");

                    b.HasKey("BlogArticleId", "ProductId");

                    b.ToTable("BlogArticleProducts");
                });

            modelBuilder.Entity("Northwind.Services.Blogging.BlogComment", b =>
                {
                    b.Property<int>("BlogArticleId")
                        .HasColumnType("int")
                        .HasColumnName("article_id");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int")
                        .HasColumnName("customer_id");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("comment");

                    b.HasKey("BlogArticleId", "CustomerId");

                    b.ToTable("BlogComments");
                });

            modelBuilder.Entity("Northwind.Services.Blogging.BlogArticleProduct", b =>
                {
                    b.HasOne("Northwind.Services.Blogging.BlogArticle", "Article")
                        .WithMany()
                        .HasForeignKey("BlogArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");
                });

            modelBuilder.Entity("Northwind.Services.Blogging.BlogComment", b =>
                {
                    b.HasOne("Northwind.Services.Blogging.BlogArticle", "Article")
                        .WithMany()
                        .HasForeignKey("BlogArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");
                });
#pragma warning restore 612, 618
        }
    }
}
