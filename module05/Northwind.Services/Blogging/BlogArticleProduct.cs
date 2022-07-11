using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Northwind.Services.Entities;

namespace Northwind.Services.Blogging
{
    public class BlogArticleProduct
    {
        [Key]
        [Column("article_id")]
        public int ArticleId { get; set; }

        [Column("title")]
        [StringLength(40)]
        public string Title { get; set; }

        [Column("text")]
        [StringLength(255)]
        public string Text { get; set; }

        [Column("publish_date")]
        public DateTime PublishDate { get; set; }

        [Column("products")]
        public List<Product> Products { get; set; }
    }
}
