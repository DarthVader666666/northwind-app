using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Northwind.Services.EntityFrameworkCore.Blogging.Context
{
    public class DesignTimeBloggingContextFactory : IDesignTimeDbContextFactory<BloggingContext>
    {
        public BloggingContext CreateDbContext(string[] args)
        {
            const string connectionStringName = "NORTHWIND_BLOGGING";
            const string connectioStringPrefix = "SQLCONNSTR_";

            var configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();
            var connectionString = configuration.GetConnectionString(connectionStringName);
            //Environment.GetEnvironmentVariable(connectioStringPrefix + connectionStringName); //configuration.GetConnectionString(connectioStringPrefix + connectionStringName);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception($"{connectioStringPrefix}{connectionStringName} environment variable is not set.");
            }

            Console.WriteLine($"Using {connectioStringPrefix}{connectionStringName} environment variable as a connection string.");

            var builderOptions = new DbContextOptionsBuilder<BloggingContext>().UseSqlServer(connectionString).Options;
            return new BloggingContext(builderOptions);
        }
    }
}
