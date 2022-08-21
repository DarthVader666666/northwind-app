using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace NorthwindMvc.Models.ProductModels
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Input value.")]
        [DisplayName("Product name")]
        public string ProductName { get; set; }

        [DisplayName("Quantity per unit")]
        public string QuantityPerUnit { get; set; }

        [Range(0.0, 5000.0, ErrorMessage = "Wrong value.")]
        [DisplayName("Unit price")]
        [BindProperty, DataType(DataType.Currency)]
        public decimal? UnitPrice { get; set; }

        [Range(0, 255, ErrorMessage = "Wrong value.")]
        [DisplayName("Units in stock")]
        public short? UnitsInStock { get; set; }

        public int? SupplierId { get; set; }

        [Required(ErrorMessage = "Choose category")]
        public int? CategoryId { get; set; }

        public bool IsSelected { get; set; }
    }
}
