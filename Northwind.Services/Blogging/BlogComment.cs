using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Services.Blogging
{
    public class BlogComment
    {
        [Column("article_id")]
        public int BlogArticleId { get; set; }

        [Column("customer_id")]
        public int CustomerId { get; set; }

        [ForeignKey(nameof(BlogArticleId))]
        public virtual BlogArticle Article { get; set; }

        [Column("comment")]
        [StringLength(255)]
        public string Comment { get; set; }
    }
}
