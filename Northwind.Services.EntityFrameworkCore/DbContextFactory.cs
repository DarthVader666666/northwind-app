using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Northwind.Services.EntityFrameworkCore.Context;

namespace Northwind.Services.EntityFrameworkCore
{
    public class DbContextFactory : IDesignTimeDbContextFactory<NorthwindContext>
    {
        private readonly IServiceProvider provider;

        public DbContextFactory()
        {
        }

        public DbContextFactory(IServiceProvider provider)
        {
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public NorthwindContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<NorthwindContext>();

            switch (args[0].ToUpper())
            {
                case "IN_MEMORY":
                    {
                        if (args.Length == 1)
                        {
                            builder.UseInMemoryDatabase("Northwind");
                        }

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
