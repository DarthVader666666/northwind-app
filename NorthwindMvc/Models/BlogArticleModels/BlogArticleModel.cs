using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace NorthwindMvc.Models.BlogArticleModels
{
    public class BlogArticleModel
    {
        public int ArticleId { get; set; }

        [Required(ErrorMessage = "Type title.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Type text.")]
        public string? Text { get; set; }

        [BindProperty, DataType(DataType.Date)]
        public DateTime PostedDate { get; set; }

        [Required(ErrorMessage = "Choose author.")]
        public int? EmployeeId { get; set; }

        public string? AuthorName { get; set; }
    }
}
