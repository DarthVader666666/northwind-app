using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Northwind.Services.EntityFrameworkCore.Context;

namespace Northwind.Services.EntityFrameworkCore
{
    public class DbContextFactory : IDesignTimeDbContextFactory<NorthwindContext>
    {

        public NorthwindContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<NorthwindContext>();

            switch (args[0].ToUpper())
            {
                case "IN_MEMORY":
                    {
                        builder.UseInMemoryDatabase("Northwind");
                    }; break;
                case "SQL":
                    {
                        if (args.Length == 2)
                        {
                            builder.UseSqlServer(args[1]);
                        }                        
                    }; break;

                    default: throw new ArgumentException();
            }

            return new NorthwindContext(builder.Options);
        }
    }
}
