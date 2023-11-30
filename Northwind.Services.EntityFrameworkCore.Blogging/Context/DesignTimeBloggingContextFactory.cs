using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
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

            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = args[0];
                Console.WriteLine($"{connectioStringPrefix}{connectionStringName} environment variable is not set. Using args.");
            }

            Console.WriteLine($"Using {connectioStringPrefix}{connectionStringName} environment variable as a connection string.");

            var builderOptions = new DbContextOptionsBuilder<BloggingContext>().UseSqlServer(connectionString).Options;
            return new BloggingContext(builderOptions);
        }
    }
}
