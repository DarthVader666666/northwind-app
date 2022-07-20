using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Services.Blogging
{    
    public partial class BlogArticleProduct
    {        
        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("article_id")]
        public int BlogArticleId { get; set; }

        [ForeignKey(nameof(BlogArticleId))]
        public virtual BlogArticle Article { get; set; }
    }
}
