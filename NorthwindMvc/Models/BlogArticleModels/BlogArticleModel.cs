using System;

namespace NorthwindMvc.Models.BlogArticleModels
{
    public class BlogArticleModel
    {
        public int ArticleId { get; set; }

        public string Title { get; set; }

        public DateTime PostedDate { get; set; }

        public int AuthorId { get; set; }

        public string AuthorName { get; set; }
    }
}
