using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Northwind.Services.EntityFrameworkCore.Entities
{
    public partial class CategorySalesFor1997Entity
    {
        [Required]
        [StringLength(15)]
        public string CategoryName { get; set; }
        [Column(TypeName = "money")]
        public decimal? CategorySales { get; set; }
    }
}
