using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Northwind.Services.EntityFrameworkCore.Entities
{
    public partial class CustomerDemographicEntity
    {
        public CustomerDemographicEntity()
        {
            CustomerCustomerDemos = new HashSet<CustomerCustomerDemoEntity>();
        }

        [Key]
        [Column("CustomerTypeID")]
        [StringLength(10)]
        public string CustomerTypeId { get; set; }
        [Column(TypeName = "ntext")]
        public string CustomerDesc { get; set; }

        [InverseProperty(nameof(CustomerCustomerDemoEntity.CustomerType))]
        public virtual ICollection<CustomerCustomerDemoEntity> CustomerCustomerDemos { get; set; }
    }
}
