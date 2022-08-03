using Northwind.Services.Products;
using Northwind.Services.Employees;
using Bogus;
using AutoMapper;
using Northwind.Services.EntityFrameworkCore.Entities;
using Northwind.Services.EntityFrameworkCore.Context;

namespace Northwind.Services.EntityFrameworkCore
{
    public static class DataSeeder
    {
        private const int NumberOfCategories = 10;
        private const int NumberOfProducts = 30;
        private const int NumberOfEmployees = 15;

        private static readonly IMapper toProductEntityMapper = new Mapper(new MapperConfiguration(conf =>
            conf.CreateMap<Product, ProductEntity>()));
        private static readonly IMapper toCategoryEntityMapper = new Mapper(new MapperConfiguration(conf =>
            conf.CreateMap<ProductCategory, CategoryEntity>()));
        private static readonly IMapper toEmployeeEntityMapper = new Mapper(new MapperConfiguration(conf =>
            conf.CreateMap<Employee, EmployeeEntity>()));

        public static void Seed(this NorthwindContext context, string picturePath)
        {
            context.Products.AddRange(GenerateProducts());
            context.Categories.AddRange(GenerateCategories(picturePath));
            context.Employees.AddRange(GenerateEmployees(picturePath));
            context.SaveChanges();
        }

        private static IEnumerable<ProductEntity> GenerateProducts()
        {
            int id = 1;

            foreach (var item in GenerateProductsCore())
            {
                item.ProductId = id++;
                yield return toProductEntityMapper.Map<ProductEntity>(item);
            }

            static IEnumerable<Product> GenerateProductsCore()
            {
                return new Faker<Product>().
                RuleFor(product => product.ProductName, faker => faker.Commerce.ProductName()).
                RuleFor(product => product.SupplierId, faker => faker.Random.Int(1, 7)).
                RuleFor(product => product.CategoryId, faker => faker.Random.Int(1, 5)).
                RuleFor(product => product.QuantityPerUnit, faker => faker.Random.Int(0, 1000).ToString()).
                RuleFor(product => product.UnitPrice, faker => Math.Round(faker.Random.Decimal(1m, 555.55m), 2)).
                RuleFor(product => product.UnitsInStock, faker => faker.Random.Short(1, 100)).
                RuleFor(product => product.UnitsOnOrder, faker => faker.Random.Short(1, 50)).
                RuleFor(product => product.ReorderLevel, faker => faker.Random.Short(1, 3)).
                RuleFor(product => product.Discontinued, faker => faker.Random.Bool()).
                Generate(NumberOfProducts);
            }
        }

        private static IEnumerable<CategoryEntity> GenerateCategories(string picturePath)
        {
            int id = 1;
            var directoryInfo = new DirectoryInfo(picturePath);
            var files = directoryInfo.GetFiles();

            foreach (var item in GenerateCategoriesCore())
            {
                var name = Array.Find(files, x => x.Name.Split('.')[0] == $"{id}");

                if (name is not null)
                {
                    using var stream = new FileStream(name.FullName, FileMode.Open, FileAccess.Read);
                    using var memoryStream = new MemoryStream();
                    Task.Run(async () => await stream?.CopyToAsync(memoryStream)).Wait();
                    item.Picture = memoryStream?.ToArray();
                }

                item.CategoryId = id++;

                yield return toCategoryEntityMapper.Map<CategoryEntity>(item);
            }

            static IEnumerable<ProductCategory> GenerateCategoriesCore()
            {
                return new Faker<ProductCategory>().
                    RuleFor(product => product.CategoryName, faker => faker.Commerce.Categories(1).First()).
                    RuleFor(product => product.Description, faker => faker.Commerce.ProductDescription()).
                    Generate(NumberOfCategories);
            }
        }

        private static IEnumerable<EmployeeEntity> GenerateEmployees(string picturePath)
        {
            int id = 1;
            var directoryInfo = new DirectoryInfo(picturePath);
            var files = directoryInfo.GetFiles();

            foreach (var item in GenerateEmployeesCore())
            {
                var name = Array.Find(files, x => x.Name.Split('.')[0] == $"e{id}");

                if (name is not null)
                {
                    using var stream = new FileStream(name.FullName, FileMode.Open, FileAccess.Read);
                    using var memoryStream = new MemoryStream();
                    Task.Run(async () => await stream?.CopyToAsync(memoryStream)).Wait();
                    item.Photo = memoryStream?.ToArray();
                }

                item.EmployeeId = id++;

                yield return toEmployeeEntityMapper.Map<EmployeeEntity>(item);
            }

            static IEnumerable<Employee> GenerateEmployeesCore()
            {
                return new Faker<Employee>().
                    RuleFor(employee => employee.LastName, faker => faker.Person.LastName).
                    RuleFor(employee => employee.FirstName, faker => faker.Person.FirstName).
                    RuleFor(employee => employee.Title, faker => faker.Company.CompanyName()).
                    RuleFor(employee => employee.BirthDate, faker => faker.Person.DateOfBirth.ToShortDateString()).
                    RuleFor(employee => employee.HireDate, faker => faker.Date.Past(new Random().Next(1, 10)).ToShortDateString()).
                    RuleFor(employee => employee.Address, faker => faker.Person.Address.Suite).
                    RuleFor(employee => employee.City, faker => faker.Person.Address.City).
                    RuleFor(employee => employee.Region, faker => faker.Address.State()).
                    RuleFor(employee => employee.PostalCode, faker => faker.Address.ZipCode()).
                    RuleFor(employee => employee.Country, faker => faker.Address.Country()).
                    RuleFor(employee => employee.HomePhone, faker => faker.Person.Phone).
                    Generate(NumberOfEmployees);
            }
        }
    }
}
