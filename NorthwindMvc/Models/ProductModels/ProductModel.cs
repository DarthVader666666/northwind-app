using System.ComponentModel.DataAnnotations;

namespace NorthwindMvc.Models.ProductModels
{
    public class ProductModel
    {
        [Required(ErrorMessage = "Input value.")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Input value.")]
        [Range(0, 5000, ErrorMessage = "Wrong value.")]
        public string QuantityPerUnit { get; set; }

        [Required(ErrorMessage = "Input value.")]
        [Range(0, 5000, ErrorMessage = "Wrong value.")]
        public decimal? UnitPrice { get; set; }

        [Required(ErrorMessage = "Input value.")]
        [Range(0, 255, ErrorMessage = "Wrong value.")]
        public short? UnitsInStock { get; set; }

        public int? SupplierId { get; set; }

        public int? CategoryId { get; set; }
    }
}
