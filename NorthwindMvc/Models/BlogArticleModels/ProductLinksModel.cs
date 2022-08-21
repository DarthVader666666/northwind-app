using System.Collections.Generic;
using NorthwindMvc.Models.ProductModels;

namespace NorthwindMvc.Models.BlogArticleModels
{
    public class ProductLinksModel
    {
        public int ArticleId { get; set; }

        public List<ProductModel> Products { get; set; }
    }
}
