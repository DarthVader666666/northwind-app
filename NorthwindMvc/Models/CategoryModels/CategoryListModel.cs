using System.Collections.Generic;

namespace NorthwindMvc.Models.CategoryModels
{
    public class CategoryListModel
    {
        public IEnumerable<CategoryModel> Categories { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
