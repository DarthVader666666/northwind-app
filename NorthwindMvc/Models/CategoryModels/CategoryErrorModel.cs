using System.Collections.Generic;

namespace NorthwindMvc.Models.CategoryModels
{
    public class CategoryErrorModel
    {
        public string CategoryName { get; set; }

        public IEnumerable<string> ProductNames { get; set; }
    }
}
