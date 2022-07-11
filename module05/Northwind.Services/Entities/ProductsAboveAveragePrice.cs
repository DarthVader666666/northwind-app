using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Services.Entities
{
    [Keyless]
    public partial class ProductsAboveAveragePrice
    {
        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }
        [Column(TypeName = "money")]
        public decimal? UnitPrice { get; set; }
    }
}
