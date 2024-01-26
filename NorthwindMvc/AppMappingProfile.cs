using System;
using AutoMapper;
using NorthwindMvc.Models.BlogArticleModels;
using NorthwindMvc.Models.CategoryModels;
using NorthwindMvc.Models.EmployeeModels;
using NorthwindMvc.Models.ProductModels;

namespace NorthwindMvc
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            this.CreateMap<Product, ProductModel>();
            this.CreateMap<Employee, EmployeeModel>();
            this.CreateMap<BlogArticle, BlogArticleModel>();
            this.CreateMap<ProductCategory, CategoryModel>();

            this.CreateMap<ProductModel, Product>();
            this.CreateMap<EmployeeModel, Employee>();
            this.CreateMap<BlogArticleModel, BlogArticle>();
            this.CreateMap<CategoryModel, ProductCategory>();
        }
    }
}
