using Northwind.Services.EntityFrameworkCore.Context;
using Northwind.Services.EntityFrameworkCore.Entities;
using Northwind.Services.Products;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Northwind.Services.EntityFrameworkCore
{
    /// <summary>
    /// Represents a stub for a product management service.
    /// </summary>
    public sealed class ProductManagementService : IProductManagementService
    {
        private readonly NorthwindContext context;
        private readonly IMapper toEntitymapper;
        private readonly IMapper fromEntitymapper;

        public ProductManagementService(NorthwindContext context)
        {
            this.context = context;
            this.toEntitymapper = new Mapper(new MapperConfiguration(conf =>
            conf.CreateMap<Product, ProductEntity>()));
            this.fromEntitymapper = new Mapper(new MapperConfiguration(conf =>
            conf.CreateMap<ProductEntity, Product>()));
        }

        public async Task<int> CreateProductAsync(Product product)
        {
            var id = 1;

            if (await this.context.Products.AnyAsync())
            {
                id = this.context.Products.MaxAsync(x => x.ProductId).Result;
                id++;
            }

            var entity = this.toEntitymapper.Map<ProductEntity>(product);

            entity.ProductId = id;
            await this.context.Products.AddAsync(entity);
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

        public async IAsyncEnumerable<Product> LookupProductsByNameAsync(List<string> names)
        {
            var products = this.context.Products.Where(p => names.Any(n => n == p.ProductName)).AsAsyncEnumerable();

            //if (!result.Any())
            //{
            //    return null;
            //}

            await foreach (var item in products)
            {
                yield return this.fromEntitymapper.Map<Product>(item);
            }
        }

        public async IAsyncEnumerable<Product> GetProductsAsync(int offset, int limit)
        { 
            var products = this.context.Products.Skip(offset).Take(limit).AsAsyncEnumerable();

            await foreach (var item in products)
            {
                yield return this.fromEntitymapper.Map<Product>(item);
            }
        }

        public async IAsyncEnumerable<Product> GetProductsForCategoryAsync(int categoryId)
        { 
            var products = this.context.Products.Where(p => p.CategoryId == categoryId).AsAsyncEnumerable();

            await foreach (var item in products)
            {
                yield return this.fromEntitymapper.Map<Product>(item);
            }
        }
        

        public async Task<(bool result, Product product)> TryGetProductAsync(int productId)
        {
            var entity = await this.context.Products.FindAsync(productId);

            return (entity is not null, this.fromEntitymapper.Map<Product>(entity));
        }

        public async Task<bool> UpdateProductAsync(int productId, Product product)
        {
            if (product.ProductId != productId)
            {
                return false;
            }

            this.context.Entry(this.toEntitymapper.Map<ProductEntity>(product)).State = EntityState.Modified;
            await this.context.SaveChangesAsync();

            return true;
        }
    }
}
