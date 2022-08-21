using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NorthwindMvc.Models.CategoryModels;

namespace NorthwindMvc.Models.ProductModels
{
    public class ProductCreateModel
    {
        public ProductModel Product { get; set; }

        [Required(ErrorMessage = "Choose category.")]
        public IEnumerable<CategoryModel> Categories { get; set; }
    }
}
