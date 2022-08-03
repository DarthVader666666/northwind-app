using Microsoft.EntityFrameworkCore;
using Northwind.Services.EntityFrameworkCore.Entities;

#nullable disable

namespace Northwind.Services.EntityFrameworkCore.Context
{
    public partial class NorthwindContext : DbContext
    {
        public NorthwindContext(DbContextOptions<NorthwindContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        public virtual DbSet<AlphabeticalListOfProductEntity> AlphabeticalListOfProducts { get; set; }
        public virtual DbSet<CategoryEntity> Categories { get; set; }
        public virtual DbSet<CategorySalesFor1997Entity> CategorySalesFor1997s { get; set; }
        public virtual DbSet<CurrentProductListEntity> CurrentProductLists { get; set; }
        public virtual DbSet<CustomerEntity> Customers { get; set; }
        public virtual DbSet<CustomerAndSuppliersByCityEntity> CustomerAndSuppliersByCities { get; set; }
        public virtual DbSet<CustomerCustomerDemoEntity> CustomerCustomerDemos { get; set; }
        public virtual DbSet<CustomerDemographicEntity> CustomerDemographics { get; set; }
        public virtual DbSet<EmployeeEntity> Employees { get; set; }
        public virtual DbSet<EmployeeTerritoryEntity> EmployeeTerritories { get; set; }
        public virtual DbSet<InvoiceEntity> Invoices { get; set; }
        public virtual DbSet<OrderEntity> Orders { get; set; }
        public virtual DbSet<OrderDetailEntity> OrderDetails { get; set; }
        public virtual DbSet<OrderDetailsExtendedEntity> OrderDetailsExtendeds { get; set; }
        public virtual DbSet<OrderSubtotalEntity> OrderSubtotals { get; set; }
        public virtual DbSet<OrdersQryEntity> OrdersQries { get; set; }
        public virtual DbSet<ProductEntity> Products { get; set; }
        public virtual DbSet<ProductSalesFor1997Entity> ProductSalesFor1997s { get; set; }
        public virtual DbSet<ProductsAboveAveragePriceEntity> ProductsAboveAveragePrices { get; set; }
        public virtual DbSet<ProductsByCategoryEntity> ProductsByCategories { get; set; }
        public virtual DbSet<QuarterlyOrderEntity> QuarterlyOrders { get; set; }
        public virtual DbSet<RegionEntity> Regions { get; set; }
        public virtual DbSet<SalesByCategoryEntity> SalesByCategories { get; set; }
        public virtual DbSet<SalesTotalsByAmountEntity> SalesTotalsByAmounts { get; set; }
        public virtual DbSet<ShipperEntity> Shippers { get; set; }
        public virtual DbSet<SummaryOfSalesByQuarterEntity> SummaryOfSalesByQuarters { get; set; }
        public virtual DbSet<SummaryOfSalesByYearEntity> SummaryOfSalesByYears { get; set; }
        public virtual DbSet<SupplierEntity> Suppliers { get; set; }
        public virtual DbSet<TerritoryEntity> Territories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AlphabeticalListOfProductEntity>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("Alphabetical list of products");
            });

            modelBuilder.Entity<CategorySalesFor1997Entity>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("Category Sales for 1997");
            });

            modelBuilder.Entity<CurrentProductListEntity>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("Current Product List");

                entity.Property(e => e.ProductId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<CustomerEntity>(entity =>
            {
                entity.Property(e => e.CustomerId).IsFixedLength(true);
            });

            modelBuilder.Entity<CustomerAndSuppliersByCityEntity>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("Customer and Suppliers by City");

                entity.Property(e => e.Relationship).IsUnicode(false);
            });

            modelBuilder.Entity<CustomerCustomerDemoEntity>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.CustomerTypeId })
                    .IsClustered(false);

                entity.Property(e => e.CustomerId).IsFixedLength(true);

                entity.Property(e => e.CustomerTypeId).IsFixedLength(true);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerCustomerDemos)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerCustomerDemo_Customers");

                entity.HasOne(d => d.CustomerType)
                    .WithMany(p => p.CustomerCustomerDemos)
                    .HasForeignKey(d => d.CustomerTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerCustomerDemo");
            });

            modelBuilder.Entity<CustomerDemographicEntity>(entity =>
            {
                entity.HasKey(e => e.CustomerTypeId)
                    .IsClustered(false);

                entity.Property(e => e.CustomerTypeId).IsFixedLength(true);
            });

            modelBuilder.Entity<EmployeeEntity>(entity =>
            {
                entity.HasOne(d => d.ReportsToNavigation)
                    .WithMany(p => p.InverseReportsToNavigation)
                    .HasForeignKey(d => d.ReportsTo)
                    .HasConstraintName("FK_Employees_Employees");
            });

            modelBuilder.Entity<EmployeeTerritoryEntity>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeId, e.TerritoryId })
                    .IsClustered(false);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeTerritories)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeTerritories_Employees");

                entity.HasOne(d => d.Territory)
                    .WithMany(p => p.EmployeeTerritories)
                    .HasForeignKey(d => d.TerritoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeTerritories_Territories");
            });

            modelBuilder.Entity<InvoiceEntity>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("Invoices");

                entity.Property(e => e.CustomerId).IsFixedLength(true);
            });

            modelBuilder.Entity<OrderEntity>(entity =>
            {
                entity.Property(e => e.CustomerId).IsFixedLength(true);

                entity.Property(e => e.Freight).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Orders_Customers");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_Orders_Employees");

                entity.HasOne(d => d.ShipViaNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ShipVia)
                    .HasConstraintName("FK_Orders_Shippers");
            });

            modelBuilder.Entity<OrderDetailEntity>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId })
                    .HasName("PK_Order_Details");

                entity.Property(e => e.Quantity).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Details_Orders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Details_Products");
            });

            modelBuilder.Entity<OrderDetailsExtendedEntity>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("Order Details Extended");
            });

            modelBuilder.Entity<OrderSubtotalEntity>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("Order Subtotals");
            });

            modelBuilder.Entity<OrdersQryEntity>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("Orders Qry");

                entity.Property(e => e.CustomerId).IsFixedLength(true);
            });

            modelBuilder.Entity<ProductEntity>(entity =>
            {
                entity.Property(e => e.ReorderLevel).HasDefaultValueSql("((0))");

                entity.Property(e => e.UnitPrice).HasDefaultValueSql("((0))");

                entity.Property(e => e.UnitsInStock).HasDefaultValueSql("((0))");

                entity.Property(e => e.UnitsOnOrder).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Products_Categories");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_Products_Suppliers");
            });

            modelBuilder.Entity<ProductSalesFor1997Entity>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("Product Sales for 1997");
            });

            modelBuilder.Entity<ProductsAboveAveragePriceEntity>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("Products Above Average Price");
            });

            modelBuilder.Entity<ProductsByCategoryEntity>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("Products by Category");
            });

            modelBuilder.Entity<QuarterlyOrderEntity>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("Quarterly Orders");

                entity.Property(e => e.CustomerId).IsFixedLength(true);
            });

            modelBuilder.Entity<RegionEntity>(entity =>
            {
                entity.HasKey(e => e.RegionId)
                    .IsClustered(false);

                entity.Property(e => e.RegionId).ValueGeneratedNever();

                entity.Property(e => e.RegionDescription).IsFixedLength(true);
            });

            modelBuilder.Entity<SalesByCategoryEntity>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("Sales by Category");
            });

            modelBuilder.Entity<SalesTotalsByAmountEntity>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("Sales Totals by Amount");
            });

            modelBuilder.Entity<SummaryOfSalesByQuarterEntity>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("Summary of Sales by Quarter");
            });

            modelBuilder.Entity<SummaryOfSalesByYearEntity>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("Summary of Sales by Year");
            });

            modelBuilder.Entity<TerritoryEntity>(entity =>
            {
                entity.HasKey(e => e.TerritoryId)
                    .IsClustered(false);

                entity.Property(e => e.TerritoryDescription).IsFixedLength(true);

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Territories)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Territories_Region");

                //if (this.Database.IsInMemory())
                //{
                //    modelBuilder.Seed();
                //}
            });

            //OnModelCreatingPartial(modelBuilder);
        }

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
