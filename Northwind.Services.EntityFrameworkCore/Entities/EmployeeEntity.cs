using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Northwind.Services.EntityFrameworkCore.Entities
{
    public partial class EmployeeEntity
    {
        public EmployeeEntity()
        {
            EmployeeTerritories = new HashSet<EmployeeTerritoryEntity>();
            InverseReportsToNavigation = new HashSet<EmployeeEntity>();
            Orders = new HashSet<OrderEntity>();
        }

        [Key]
        [Column("EmployeeID")]
        public int EmployeeId { get; set; }
        [Required]
        [StringLength(20)]
        public string LastName { get; set; }
        [Required]
        [StringLength(10)]
        public string FirstName { get; set; }
        [StringLength(30)]
        public string Title { get; set; }
        [StringLength(25)]
        public string TitleOfCourtesy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? BirthDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? HireDate { get; set; }
        [StringLength(60)]
        public string Address { get; set; }
        [StringLength(15)]
        public string City { get; set; }
        [StringLength(15)]
        public string Region { get; set; }
        [StringLength(10)]
        public string PostalCode { get; set; }
        [StringLength(15)]
        public string Country { get; set; }
        [StringLength(24)]
        public string HomePhone { get; set; }
        [StringLength(4)]
        public string Extension { get; set; }
        [Column(TypeName = "image")]
        public byte[] Photo { get; set; }
        [Column(TypeName = "ntext")]
        public string Notes { get; set; }
        public int? ReportsTo { get; set; }
        [StringLength(255)]
        public string PhotoPath { get; set; }

        [ForeignKey(nameof(ReportsTo))]
        [InverseProperty(nameof(EmployeeEntity.InverseReportsToNavigation))]
        public virtual EmployeeEntity ReportsToNavigation { get; set; }
        [InverseProperty(nameof(EmployeeTerritoryEntity.Employee))]
        public virtual ICollection<EmployeeTerritoryEntity> EmployeeTerritories { get; set; }
        [InverseProperty(nameof(EmployeeEntity.ReportsToNavigation))]
        public virtual ICollection<EmployeeEntity> InverseReportsToNavigation { get; set; }
        [InverseProperty(nameof(OrderEntity.Employee))]
        public virtual ICollection<OrderEntity> Orders { get; set; }
    }
}
