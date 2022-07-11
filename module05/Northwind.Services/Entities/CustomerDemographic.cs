using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Services.Entities
{
    public partial class CustomerDemographic
    {
        public CustomerDemographic()
        {
            Customers = new HashSet<Customer>();
        }

        [Key]
        [Column("CustomerTypeID")]
        [StringLength(10)]
        public string CustomerTypeId { get; set; }
        [Column(TypeName = "ntext")]
        public string CustomerDesc { get; set; }

        [ForeignKey("CustomerTypeId")]
        [InverseProperty("CustomerTypes")]
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
