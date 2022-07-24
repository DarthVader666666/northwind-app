using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Northwind.Services.EntityFrameworkCore.Entities
{
    [Table("Region")]
    public partial class RegionEntity
    {
        public RegionEntity()
        {
            Territories = new HashSet<TerritoryEntity>();
        }

        [Key]
        [Column("RegionID")]
        public int RegionId { get; set; }
        [Required]
        [StringLength(50)]
        public string RegionDescription { get; set; }

        [InverseProperty(nameof(TerritoryEntity.Region))]
        public virtual ICollection<TerritoryEntity> Territories { get; set; }
    }
}
