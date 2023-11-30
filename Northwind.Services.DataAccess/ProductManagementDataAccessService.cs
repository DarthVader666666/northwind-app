using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;
using Northwind.Services.Products;
using Northwind.DataAccess;
using Northwind.DataAccess.Products;

namespace Northwind.Services.DataAccess
{
    public class ProductManagementDataAccessService : IProductManagementService
    {
        private readonly IProductDataAccessObject productService;
        private readonly Mapper mapper;

        public ProductManagementDataAccessService(NorthwindDataAccessFactory factory)
        {
            this.productService = factory.GetProductDataAccessObject();
            this.mapper = new Mapper(new MapperConfiguration(conf =>
            conf.CreateMap<ProductTransferObject, Product>()));
        }

        public Task<int> CreateProductAsync(Product product)
        {
            var mapper = new Mapper(new MapperConfiguration(conf =>
            conf.CreateMap<Product, ProductTransferObject>()));

            var productTO = mapper.Map<ProductTransferObject>(product);

            return Task.Run(() => this.productService.InsertProduct(productTO));
        }

        public Task<bool> DestroyProductAsync(int productId)
        {
            return Task.Run(() => this.productService.DeleteProduct(productId));
        }

        public async IAsyncEnumerable<Product> LookupProductsByNameAsync(List<string> names)
        {
            await foreach (var item in this.productService.SelectProductsByName(names))
            {
                yield return this.mapper.Map<Product>(item);
            }
        }

        public async IAsyncEnumerable<Product> GetProductsAsync(int offset, int limit)
        {
            await foreach (var item in this.productService.SelectProducts(offset, limit))
            {
                yield return this.mapper.Map<Product>(item);
            }
        }

        public async IAsyncEnumerable<Product> GetProductsForCategoryAsync(int categoryId)
        {
            await foreach (var item in this.productService.SelectProductByCategory(new List<int> { categoryId }))
            {
                yield return this.mapper.Map<Product>(item);
            }
        }

        public Task<(bool, Product)> TryGetProductAsync(int productId)
        {
            return Task.Run(() =>
            {
                var product = this.productService.FindProduct(productId);
                return (product is not null, this.mapper.Map<Product>(product));
            });
        }

        public Task<bool> UpdateProductAsync(int productId, Product product)
        {
            var mapper = new Mapper(new MapperConfiguration(conf =>
            conf.CreateMap<Product, ProductTransferObject>()));

            return Task.Run(() =>
            {
                product.ProductId = productId;
                var productTO = mapper.Map<ProductTransferObject>(product);
                return this.productService.UpdateProduct(productTO);
            });
        }

        public Task<int> GetProductsCountAsync()
        {
            return Task.Run(() => this.productService.GetAmountOfProducts());
        }
    }
}
