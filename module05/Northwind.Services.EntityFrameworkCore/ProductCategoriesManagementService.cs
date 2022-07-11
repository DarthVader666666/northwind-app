using Microsoft.EntityFrameworkCore;
using Northwind.Services.Entities;
using Northwind.Services.EntityFrameworkCore.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Services.EntityFrameworkCore
{
    public class ProductCategoriesManagementService : IProductCategoriesManagementService
    {
        private readonly NorthwindContext context;

        public ProductCategoriesManagementService(NorthwindContext context)
        {
            this.context = context;
        }

        public async Task<int> CreateCategoryAsync(Category productCategory)
        {
            var id = 1;

            if (this.context.Categories.AnyAsync().Result)
            {
                id = this.context.Categories.Max(x => x.CategoryId);
                id++;
            }

            productCategory.CategoryId = id;
            await this.context.Categories.AddAsync(productCategory);
            await this.context.SaveChangesAsync();

            return id;
        }

        public async Task<bool> DestroyCategoryAsync(int categoryId)
        {
            var category = this.context.Categories.FindAsync(categoryId).Result;

            if (category is null)
            {
                return false;
            }

            this.context.Categories.Remove(category);
            await this.context.SaveChangesAsync();
            return true;
        }

        public IAsyncEnumerable<Category> LookupCategoriesByNameAsync(ICollection<string> names)
        {
            var categories =  this.context.Categories.Where(c => names.Any(n => n == c.CategoryName));

            if (!categories.Any())
            {
                return null;
            }

            return categories.AsAsyncEnumerable();
        }

        public IAsyncEnumerable<Category> GetCategoriesAsync(int offset, int limit)
        {
            return this.context.Categories.Skip(offset).Take(limit).AsAsyncEnumerable();
        }

        public async Task<(bool, Category)> TryGetCategoryAsync(int categoryId)
        {
            var productCategory = await Task.Run(async () =>
            await this.context.Categories.FindAsync(categoryId));

            return (productCategory is not null, productCategory);
        }

        public async Task<bool> UpdateCategoriesAsync(int categoryId, Category productCategory)
        {
            if (productCategory.CategoryId != categoryId)
            {
                return false;
            }
            
            this.context.Entry(productCategory).State = EntityState.Modified;
            await this.context.SaveChangesAsync();

            return true;
        }
    }
}
