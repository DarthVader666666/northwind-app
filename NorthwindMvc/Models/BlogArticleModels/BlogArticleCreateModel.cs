using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NorthwindMvc.Models.EmployeeModels;

namespace NorthwindMvc.Models.BlogArticleModels
{
    public class BlogArticleCreateModel
    {
        public BlogArticleModel BlogArticle { get; set; }

        [Required(ErrorMessage = "Choose category.")]
        public IEnumerable<EmployeeModel> Authors { get; set; }
    }
}
