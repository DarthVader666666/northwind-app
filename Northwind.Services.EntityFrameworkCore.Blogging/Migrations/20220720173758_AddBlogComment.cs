using Microsoft.EntityFrameworkCore.Migrations;

namespace Northwind.Services.EntityFrameworkCore.Blogging.Migrations
{
    public partial class AddBlogComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogComments",
                columns: table => new
                {
                    article_id = table.Column<int>(type: "int", nullable: false),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogComments", x => new { x.article_id, x.customer_id });
                    table.ForeignKey(
                        name: "FK_BlogComments_BlogArticles_article_id",
                        column: x => x.article_id,
                        principalTable: "BlogArticles",
                        principalColumn: "article_id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogComments");
        }
    }
}
