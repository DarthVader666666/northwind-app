using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Services.Blogging
{
    public class BlogComment
    {
        [Key]
        [Column("blog_comment_id")]
        public int BlogCommentId { get; set; }

        [Column("article_id")]
        public int ArticleId { get; set; }

        [Column("customer_id")]
        public int CustomerId { get; set; }

        [Column("comment")]
        [StringLength(255)]
        public string Comment { get; set; }
    }
}
