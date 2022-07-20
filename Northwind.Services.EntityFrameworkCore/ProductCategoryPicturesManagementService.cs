using Northwind.Services.EntityFrameworkCore.Context;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Services.EntityFrameworkCore
{
    public class ProductCategoryPicturesManagementService : IProductCategoryPicturesManagementService
    {
        private readonly NorthwindContext context;

        public ProductCategoryPicturesManagementService(NorthwindContext context)
        {
            this.context = context;
        }

        public async Task<bool> DestroyPictureAsync(int categoryId)
        {
            var category = await this.context.Categories.FindAsync(categoryId);

            if (category is null)
            {
                return false;
            }

            category.Picture = null;
            await this.context.SaveChangesAsync();
            return true;
        }

        public async Task<(bool, byte[])> TryGetPictureAsync(int categoryId)
        {
            var category = await Task.Run(async () =>
            await this.context.Categories.FindAsync(categoryId));

            return (category.Picture is not null, category.Picture);
        }

        public async Task<bool> UpdatePictureAsync(int categoryId, Stream stream)
        {
            var category = await this.context.Categories.FindAsync(categoryId);

            if (category is null)
            {
                return false;
            }

            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            category.Picture = memoryStream.ToArray();

            this.context.Entry(category).State = EntityState.Modified;
            await this.context.SaveChangesAsync();

            return category.Picture.Length > 0;
        }
    }
}
