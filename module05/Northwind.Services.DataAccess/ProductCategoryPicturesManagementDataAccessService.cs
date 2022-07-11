using System;
using System.IO;
using System.Threading.Tasks;
using Northwind.DataAccess;
using Northwind.DataAccess.Products;

namespace Northwind.Services.DataAccess
{
    public class ProductCategoryPicturesManagementDataAccessService : IProductCategoryPicturesManagementService
    {
        private const string OleHeader = "15-1C-2F-00-02-00-00-00-0D-00-0E-00-14-00-21-00-FF-FF-FF-FF-42-69-74-6D-61-70-20-49-6D-61-67-65-00-50-61-69-6E-74-2E-50-69-63-74-75-72-65-00-01-05-00-00-02-00-00-00-07-00-00-00-50-42-72-75-73-68-00-00-00-00-00-00-00-00-00-A0-29-00-00";

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

                if (bytes is null || bytes.Length == 0)
                {
                    return (false, null);
                }

                if (BitConverter.ToString(bytes[..78]) == OleHeader)
                {
                    bytes = bytes[78..];
                }

                return (bytes is not null, bytes);
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
