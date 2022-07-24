using Microsoft.EntityFrameworkCore;
using Northwind.Services.Products;
using Northwind.Services.EntityFrameworkCore.Context;
using Northwind.Services.EntityFrameworkCore.Entities;
using AutoMapper;

namespace Northwind.Services.EntityFrameworkCore
{
    public class ProductCategoriesManagementService : IProductCategoriesManagementService
    {
        private readonly NorthwindContext context;
        private readonly IMapper toEntitymapper;
        private readonly IMapper fromEntitymapper;

        public ProductCategoriesManagementService(NorthwindContext context)
        {
            this.context = context;
            this.toEntitymapper = new Mapper(new MapperConfiguration(conf =>
            conf.CreateMap<ProductCategory, CategoryEntity>()));
            this.fromEntitymapper = new Mapper(new MapperConfiguration(conf =>
            conf.CreateMap<CategoryEntity, ProductCategory>()));
        }

        public async Task<int> CreateCategoryAsync(ProductCategory productCategory)
        {
            var id = 1;

            if (this.context.Categories.AnyAsync().Result)
            {
                id = this.context.Categories.Max(x => x.CategoryId);
                id++;
            }

            var entity = this.toEntitymapper.Map<CategoryEntity>(productCategory);

            entity.CategoryId = id;
            await this.context.Categories.AddAsync(entity);
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

        public async IAsyncEnumerable<ProductCategory> LookupCategoriesByNameAsync(ICollection<string> names)
        {
            var categories = this.context.Categories.Where(c => names.Any(n => n == c.CategoryName)).AsAsyncEnumerable();

            //if (!categories.Any())
            //{
            //    return null;
            //}

            await foreach (var item in categories)
            {
                yield return this.fromEntitymapper.Map<ProductCategory>(item);
            }
        }

        public async IAsyncEnumerable<ProductCategory> GetCategoriesAsync(int offset, int limit)
        {
            var categories = this.context.Categories.Skip(offset).Take(limit).AsAsyncEnumerable();

            await foreach (var item in categories)
            {
                yield return this.fromEntitymapper.Map<ProductCategory>(item);
            }
        }

        public async Task<(bool, ProductCategory)> TryGetCategoryAsync(int categoryId)
        {
            var entity = await this.context.Categories.FindAsync(categoryId);

            return (entity is not null, this.fromEntitymapper.Map< ProductCategory>(entity));
        }

        public async Task<bool> UpdateCategoriesAsync(int categoryId, ProductCategory productCategory)
        {
            if (productCategory.CategoryId != categoryId)
            {
                return false;
            }

            this.context.Entry(this.toEntitymapper.Map<CategoryEntity>(productCategory)).State = EntityState.Modified;
            await this.context.SaveChangesAsync();

            return true;
        }
    }
}
