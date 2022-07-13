using System.Collections.Generic;
using System.Threading.Tasks;
using Northwind.DataAccess;
using Northwind.DataAccess.Products;
using Northwind.Services;
using Northwind.Services.Entities;
using AutoMapper;

namespace Northwind.Services.DataAccess
{
    public class ProductCategoriesManagementDataAccessService : IProductCategoriesManagementService
    {
        private readonly IProductCategoryDataAccessObject categoryService;
        private readonly Mapper mapper;

        public ProductCategoriesManagementDataAccessService(NorthwindDataAccessFactory factory)
        {
            this.categoryService = factory.GetProductCategoryDataAccessObject();
            this.mapper = new Mapper(new MapperConfiguration(conf =>
            conf.CreateMap<ProductCategoryTransferObject, Category>()));
        }

        public Task<int> CreateCategoryAsync(Category productCategory)
        {
            var mapper = new Mapper(new MapperConfiguration(conf =>
            conf.CreateMap<Category, ProductCategoryTransferObject>()));

            var category = mapper.Map<ProductCategoryTransferObject>(productCategory);

            return Task.Run(() => this.categoryService.InsertProductCategory(category));
        }

        public Task<bool> DestroyCategoryAsync(int categoryId)
        {
            return Task.Run(() => this.categoryService.DeleteProductCategory(categoryId));
        }

        public async IAsyncEnumerable<Category> LookupCategoriesByNameAsync(ICollection<string> names)
        {
            await foreach (var item in this.categoryService.SelectProductCategoriesByName(names))
            {
                yield return this.mapper.Map<Category>(item);
            }
        }

        public async IAsyncEnumerable<Category> GetCategoriesAsync(int offset, int limit)
        {
            await foreach (var item in this.categoryService.SelectProductCategories(offset, limit))
            { 
                yield return this.mapper.Map<Category>(item);
            }
        }

        public Task<(bool, Category)> TryGetCategoryAsync(int categoryId)
        {
            return Task.Run(() =>
            {
                var category = this.categoryService.FindProductCategory(categoryId);
                return (category is not null, this.mapper.Map<Category>(category));
            });
        }

        public Task<bool> UpdateCategoriesAsync(int categoryId, Category productCategory)
        {
            return Task.Run(() =>
            {
                productCategory.CategoryId = categoryId;

                var mapper = new Mapper(new MapperConfiguration(conf =>
                conf.CreateMap<Category, ProductCategoryTransferObject>()));

                var category = mapper.Map<ProductCategoryTransferObject>(productCategory); 

                return this.categoryService.UpdateProductCategory(category);
            });
        }
    }
}
