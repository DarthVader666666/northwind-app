using System;

namespace NorthwindMvc.Models
{
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)this.TotalItems / this.ItemsPerPage);
        public string? Category { get; set; }
        public string? Name { get; set; }
    }
}
