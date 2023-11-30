using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using Northwind.DataAccess;
using Northwind.DataAccess.Products;

namespace Northwind.Services.DataAccess
{
    public class ProductCategoryPicturesManagementDataAccessService : IProductCategoryPicturesManagementService
    {
        private readonly IProductCategoryDataAccessObject categoryService;

        public ProductCategoryPicturesManagementDataAccessService(NorthwindDataAccessFactory factory)
        {
            this.categoryService = factory.GetProductCategoryDataAccessObject();
        }

        public Task<bool> DestroyPictureAsync(int categoryId)
        {
            return Task.Run(() =>
            {
                var category = this.categoryService.FindProductCategory(categoryId);
                category.Picture = null;
                return this.categoryService.UpdateProductCategory(category);
            });
        }

        public Task<(bool, byte[])> TryGetPictureAsync(int categoryId)
        {
            return Task.Run(() =>
            {
                var category = this.categoryService.FindProductCategory(categoryId);
                var bytes = category.Picture;

                if (bytes == null || bytes.Length == 0)
                {
                    return (false, null);
                }

                return (true, bytes);
            });
        }

        public Task<bool> UpdatePictureAsync(int categoryId, Stream stream)
        {
            return Task.Run(() =>
            {
                if (stream is null)
                {
                    return false;
                }

                var memoryStream = new MemoryStream();
                stream.CopyToAsync(memoryStream);

                var bytes = memoryStream.ToArray();
                var category = this.categoryService.FindProductCategory(categoryId);
                category.Picture = memoryStream.ToArray();

                return this.categoryService.UpdateProductCategory(category);
            });
        }
    }
}
