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
using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

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
            switch (this.Configuration["DataStorage"].ToUpper())
            {
                case "SQL":
                    {
                        services.AddScoped((service) =>
                        {
                            var sqlConnection =
                            new SqlConnection(this.Configuration["ConnectionString"]);
                            sqlConnection.Open();
                            return sqlConnection;
                        });

                        services.AddScoped<NorthwindDataAccessFactory, SqlServerDataAccessFactory>(
                            serviceProvider => new SqlServerDataAccessFactory(
                                new SqlConnection(this.Configuration["ConnectionString"])));

                        services.AddScoped<IProductManagementService, ProductManagementDataAccessService>(
                            serviceProvider => new ProductManagementDataAccessService(serviceProvider.GetService<NorthwindDataAccessFactory>()));

                        services.AddScoped<IProductCategoriesManagementService, ProductCategoriesManagementDataAccessService>(
                            serviceProvider => new ProductCategoriesManagementDataAccessService(serviceProvider.GetService<NorthwindDataAccessFactory>()));

                        services.AddScoped<IProductCategoryPicturesManagementService, ProductCategoryPicturesManagementDataAccessService>(
                            serviceProvider => new ProductCategoryPicturesManagementDataAccessService(serviceProvider.GetService<NorthwindDataAccessFactory>()));

                        services.AddScoped<IEmployeeManagementService, EmployeeManagementDataAccessService>(
                            serviceProvider => new EmployeeManagementDataAccessService(serviceProvider.GetService<NorthwindDataAccessFactory>()));

                        services.AddScoped<IBloggingService, BloggingService>(
                            serviceProvider => new BloggingService(new DesignTimeBloggingContextFactory()));

                        services.AddScoped<NorthwindDataAccessFactory, SqlServerDataAccessFactory>();
                    }; break;

                case "IN_MEMORY":
                    {
                        services.AddDbContext<NorthwindContext>(opt => opt.UseInMemoryDatabase("Northwind"));

                        services.AddTransient<IProductManagementService, ProductManagementService>(
                            serviceProvider => new ProductManagementService(serviceProvider.GetService<NorthwindContext>()));

                        services.AddTransient<IProductCategoriesManagementService, ProductCategoriesManagementService>(
                            serviceProvider => new ProductCategoriesManagementService(serviceProvider.GetService<NorthwindContext>()));

                        services.AddTransient<IProductCategoryPicturesManagementService, ProductCategoryPicturesManagementService>(
                            serviceProvider => new ProductCategoryPicturesManagementService(serviceProvider.GetService<NorthwindContext>()));

                        services.AddTransient<IEmployeeManagementService, EmployeeManagementService>(
                            serviceProvider => new EmployeeManagementService(serviceProvider.GetService<NorthwindContext>()));
                    };break;
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

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
