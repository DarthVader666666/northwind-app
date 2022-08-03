using Northwind.Services;
using Northwind.Services.Employees;
using Northwind.Services.Blogging;
using Northwind.Services.DataAccess;
using Northwind.Services.EntityFrameworkCore;
using Northwind.Services.EntityFrameworkCore.Context;
using Northwind.Services.EntityFrameworkCore.Blogging;
using Northwind.Services.EntityFrameworkCore.Blogging.Context;
using Northwind.DataAccess;
using Northwind.DataAccess.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;

namespace NorthwindApiApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BloggingContext>(options => 
            options.UseSqlServer(this.Configuration.GetConnectionString("NorthwindBlogging")));

            services.AddScoped<IBloggingService, BloggingService>(serviceProvider =>
            new BloggingService(serviceProvider.GetService<BloggingContext>()));

            switch (this.Configuration["DataStorage"].ToUpper())
            {
                case "ADO_SQL":
                    {
                        services.AddScoped<NorthwindDataAccessFactory, SqlServerDataAccessFactory>(
                            serviceProvider => new SqlServerDataAccessFactory(
                                this.Configuration.GetConnectionString("Northwind")));

                        services.AddScoped<IProductManagementService, ProductManagementDataAccessService>(
                            serviceProvider => new ProductManagementDataAccessService(
                                serviceProvider.GetService<NorthwindDataAccessFactory>()));

                        services.AddScoped<IProductCategoriesManagementService, ProductCategoriesManagementDataAccessService>(
                            serviceProvider => new ProductCategoriesManagementDataAccessService(
                                serviceProvider.GetService<NorthwindDataAccessFactory>()));

                        services.AddScoped<IProductCategoryPicturesManagementService, ProductCategoryPicturesManagementDataAccessService>(
                            serviceProvider => new ProductCategoryPicturesManagementDataAccessService(
                                serviceProvider.GetService<NorthwindDataAccessFactory>()));

                        services.AddScoped<IEmployeeManagementService, EmployeeManagementDataAccessService>(
                            serviceProvider => new EmployeeManagementDataAccessService(
                                serviceProvider.GetService<NorthwindDataAccessFactory>()));
                    }; break;

                case "EF_SQL":
                    {
                        services.AddDbContext<NorthwindContext>(options =>
                        options.UseSqlServer(this.Configuration.GetConnectionString("Northwind")));

                        services.AddScoped<IProductManagementService, ProductManagementService>(
                            serviceProvider => new ProductManagementService(
                                serviceProvider.GetService<NorthwindContext>()));

                        services.AddScoped<IProductCategoriesManagementService, ProductCategoriesManagementService>(
                            serviceProvider => new ProductCategoriesManagementService(
                                serviceProvider.GetService<NorthwindContext>()));

                        services.AddScoped<IProductCategoryPicturesManagementService, ProductCategoryPicturesManagementService>(
                            serviceProvider => new ProductCategoryPicturesManagementService(
                                serviceProvider.GetService<NorthwindContext>()));

                        services.AddScoped<IEmployeeManagementService, EmployeeManagementService>(
                            serviceProvider => new EmployeeManagementService(
                                 serviceProvider.GetService<NorthwindContext>()));
                    };break;

                case "IN_MEMORY":
                    {
                        services.AddTransient<NorthwindContext>(ser => new DbContextFactory().
                        CreateDbContext(new string[] { "IN_MEMORY" }));

                        services.AddScoped<IProductManagementService, ProductManagementService>(
                            serviceProvider => new ProductManagementService(serviceProvider.GetService<NorthwindContext>()));

                        services.AddScoped<IProductCategoriesManagementService, ProductCategoriesManagementService>(
                            serviceProvider => new ProductCategoriesManagementService(serviceProvider.GetService<NorthwindContext>()));

                        services.AddScoped<IProductCategoryPicturesManagementService, ProductCategoryPicturesManagementService>(
                            serviceProvider => new ProductCategoryPicturesManagementService(serviceProvider.GetService<NorthwindContext>()));

                        services.AddScoped<IEmployeeManagementService, EmployeeManagementService>(
                            serviceProvider => new EmployeeManagementService(serviceProvider.GetService<NorthwindContext>()));
                    }; break;
            }

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            try
            {
                var context = app.ApplicationServices.GetService<NorthwindContext>();

                if (context.Database.IsInMemory())
                {
                    context.Seed(this.Configuration["PicturePath"]);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("DB Context is scoped.");
            }

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
