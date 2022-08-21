using System.ComponentModel.DataAnnotations.Schema;

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
