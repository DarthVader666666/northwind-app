using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Northwind.Services.EntityFrameworkCore.Blogging.Migrations
{
    public partial class AddBlogArticleProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogArticleProducts",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "int", nullable: false),
                    article_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogArticleProducts", x => new { x.article_id, x.product_id });
                    table.ForeignKey(
                        name: "FK_BlogArticleProducts_BlogArticles_article_id",
                        column: x => x.article_id,
                        principalTable: "BlogArticles",
                        principalColumn: "article_id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogArticleProducts");
        }
    }
}
