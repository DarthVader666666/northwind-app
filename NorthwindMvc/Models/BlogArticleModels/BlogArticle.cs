using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace NorthwindMvc.Models.BlogArticleModels
{
    public class BlogArticle
    {
        public int ArticleId { get; set; }

        public string? Title { get; set; }

        public string? Text { get; set; }

        public DateTime? PublishDate { get; set; }

        public int? EmployeeId { get; set; }
    }
}
