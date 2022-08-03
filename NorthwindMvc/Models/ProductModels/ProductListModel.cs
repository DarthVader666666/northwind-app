using System.Collections.Generic;

namespace NorthwindMvc.Models.ProductModels
{
    public class ProductListModel
    {
        public IEnumerable<ProductModel> Products { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
