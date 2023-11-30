﻿using System;
using AutoMapper;
using Northwind.Services.Blogging;
using Northwind.Services.Employees;
using Northwind.Services.Products;
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
            this.CreateMap<Employee, EmployeeModel>().
                ForMember(e => e.HireDate, opt => opt.MapFrom(e => DateTime.ParseExact(e.HireDate, "MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture))).
                ForMember(e => e.BirthDate, opt => opt.MapFrom(e => DateTime.ParseExact(e.BirthDate, "MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)));
            this.CreateMap<BlogArticle, BlogArticleModel>();
            this.CreateMap<ProductCategory, CategoryModel>();

            this.CreateMap<ProductModel, Product>();
            this.CreateMap<EmployeeModel, Employee>();
            this.CreateMap<BlogArticleModel, BlogArticle>();
            this.CreateMap<CategoryModel, ProductCategory>();
        }
    }
}
