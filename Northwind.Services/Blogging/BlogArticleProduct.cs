using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Services.Blogging
{
    public partial class BlogArticleProduct
    {
        [Key]
        [Column("article_product_id")]
        public int ArticleProductId { get; set; }

        [Column("article_id")]
        public int ArticleId { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }
    }
}
