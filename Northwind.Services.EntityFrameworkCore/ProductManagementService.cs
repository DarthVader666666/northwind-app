using Northwind.Services.EntityFrameworkCore.Context;
using Northwind.Services.Entities;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Services.EntityFrameworkCore
{
    /// <summary>
    /// Represents a stub for a product management service.
    /// </summary>
    public sealed class ProductManagementService : IProductManagementService
    {
        private readonly NorthwindContext context;

        public ProductManagementService(NorthwindContext context)
        {
            this.context = context;
        }

        public async Task<int> CreateProductAsync(Product product)
        {
            var id = 1;

            if (await this.context.Products.AnyAsync())
            {
                id = this.context.Products.MaxAsync(x => x.ProductId).Result;
                id++;
            }

            product.ProductId = id;
            await this.context.Products.AddAsync(product);
            await this.context.SaveChangesAsync();

            return id;
        }

        public async Task<bool> DestroyProductAsync(int productId)
        {
            var product = this.context.Products.FindAsync(productId).Result;

            if (product is null)
            {
                return false;
            }

            this.context.Products.Remove(product);
            await this.context.SaveChangesAsync();
            return true;
        }

        public IAsyncEnumerable<Product> LookupProductsByNameAsync(List<string> names)
        {
            var result = this.context.Products.Where(p => names.Any(n => n == p.ProductName));

            if (!result.Any())
            {
                return null;
            }

            return result.AsAsyncEnumerable();
        }

        public IAsyncEnumerable<Product> GetProductsAsync(int offset, int limit) =>
            this.context.Products.Skip(offset).Take(limit).AsAsyncEnumerable();

        public IAsyncEnumerable<Product> GetProductsForCategoryAsync(int categoryId) =>
            this.context.Products.Where(p => p.CategoryId == categoryId).AsAsyncEnumerable();

        public async Task<(bool result, Product product)> TryGetProductAsync(int productId)
        {
            var product = await this.context.Products.FindAsync(productId);
            return (product is not null, product);
        }

        public async Task<bool> UpdateProductAsync(int productId, Product product)
        {
            if (product.ProductId != productId)
            {
                return false;
            }

            this.context.Entry(product).State = EntityState.Modified;
            await this.context.SaveChangesAsync();

            return true;
        }
    }
}
