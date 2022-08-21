using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace NorthwindMvc.Models.CategoryModels
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is missing.")]
        [DisplayName("Category name")]
        public string CategoryName { get; set; }

        public string Description { get; set; }

        public byte[] Picture { get; set; }

        public string PictureHexString { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile PictureForm { get; set; }
    }
}
