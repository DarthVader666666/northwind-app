using System.Collections.Generic;

namespace NorthwindMvc.Models.BlogArticleModels
{
    public class BlogArticleListModel
    {
        public IEnumerable<BlogArticleModel> Articles { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
