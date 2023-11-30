using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

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
            var entity = this.toEntitymapper.Map<CategoryEntity>(productCategory);

            await this.context.Categories.AddAsync(entity);
            await this.context.SaveChangesAsync();

            return entity.CategoryId;
        }

        public async Task<bool> DestroyCategoryAsync(int categoryId)
        {
            var category = this.context.Categories.FindAsync(categoryId).Result;

            if (category is null)
            {
                return false;
            }

            try
            {
                this.context.Categories.Remove(category);
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public async IAsyncEnumerable<ProductCategory> LookupCategoriesByNameAsync(ICollection<string> names)
        {
            var categories = this.context.Categories.AsAsyncEnumerable();

            await foreach (var c in categories)
            {
                if (names.Any(x =>
                {
                    if (x is null) return false;
                    return c.CategoryName.Contains(x, StringComparison.OrdinalIgnoreCase);
                }))
                {
                    yield return this.fromEntitymapper.Map<ProductCategory>(c);
                }
            }
        }

        public async IAsyncEnumerable<ProductCategory> GetCategoriesAsync(int offset, int limit)
        {
            var categories = this.context.Categories.OrderBy(x => x.CategoryId).Skip(offset).Take(limit).AsAsyncEnumerable();

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

        public async Task<int> GetCategoriesAmountAsync()
        {
            return await this.context.Categories.CountAsync();
        }
    }
}
