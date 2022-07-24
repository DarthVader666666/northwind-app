﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Northwind.Services.EntityFrameworkCore.Entities
{
    public partial class QuarterlyOrderEntity
    {
        [Column("CustomerID")]
        [StringLength(5)]
        public string CustomerId { get; set; }
        [StringLength(40)]
        public string CompanyName { get; set; }
        [StringLength(15)]
        public string City { get; set; }
        [StringLength(15)]
        public string Country { get; set; }
    }
}
