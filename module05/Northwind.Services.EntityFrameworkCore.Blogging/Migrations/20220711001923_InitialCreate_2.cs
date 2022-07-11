using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Northwind.Services.EntityFrameworkCore.Blogging.Migrations
{
    public partial class InitialCreate_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "product_id",
                table: "BlogArticleProducts");

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Description = table.Column<string>(type: "ntext", nullable: true),
                    Picture = table.Column<byte[]>(type: "image", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerID = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ContactTitle = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    City = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Region = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "CustomerDemographic",
                columns: table => new
                {
                    CustomerTypeID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CustomerDesc = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDemographic", x => x.CustomerTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TitleOfCourtesy = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    HireDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    City = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Region = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    HomePhone = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Photo = table.Column<byte[]>(type: "image", nullable: true),
                    Notes = table.Column<string>(type: "ntext", nullable: true),
                    ReportsTo = table.Column<int>(type: "int", nullable: true),
                    PhotoPath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK_Employee_Employee_ReportsTo",
                        column: x => x.ReportsTo,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    RegionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegionDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.RegionID);
                });

            migrationBuilder.CreateTable(
                name: "Shipper",
                columns: table => new
                {
                    ShipperID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipper", x => x.ShipperID);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    SupplierID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ContactTitle = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    City = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Region = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    HomePage = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.SupplierID);
                });

            migrationBuilder.CreateTable(
                name: "CustomerCustomerDemographic",
                columns: table => new
                {
                    CustomerId = table.Column<string>(type: "nvarchar(5)", nullable: false),
                    CustomerTypeId = table.Column<string>(type: "nvarchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerCustomerDemographic", x => new { x.CustomerId, x.CustomerTypeId });
                    table.ForeignKey(
                        name: "FK_CustomerCustomerDemographic_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerCustomerDemographic_CustomerDemographic_CustomerTypeId",
                        column: x => x.CustomerTypeId,
                        principalTable: "CustomerDemographic",
                        principalColumn: "CustomerTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Territory",
                columns: table => new
                {
                    TerritoryID = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TerritoryDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RegionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Territory", x => x.TerritoryID);
                    table.ForeignKey(
                        name: "FK_Territory_Region_RegionID",
                        column: x => x.RegionID,
                        principalTable: "Region",
                        principalColumn: "RegionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    EmployeeID = table.Column<int>(type: "int", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    RequiredDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ShippedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ShipVia = table.Column<int>(type: "int", nullable: true),
                    Freight = table.Column<decimal>(type: "money", nullable: true),
                    ShipName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    ShipAddress = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    ShipCity = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ShipRegion = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ShipPostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ShipCountry = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Order_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK_Order_Employee_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_Order_Shipper_ShipVia",
                        column: x => x.ShipVia,
                        principalTable: "Shipper",
                        principalColumn: "ShipperID");
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    SupplierID = table.Column<int>(type: "int", nullable: true),
                    CategoryID = table.Column<int>(type: "int", nullable: true),
                    QuantityPerUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    UnitPrice = table.Column<decimal>(type: "money", nullable: true),
                    UnitsInStock = table.Column<short>(type: "smallint", nullable: true),
                    UnitsOnOrder = table.Column<short>(type: "smallint", nullable: true),
                    ReorderLevel = table.Column<short>(type: "smallint", nullable: true),
                    Discontinued = table.Column<bool>(type: "bit", nullable: false),
                    BlogArticleProductArticleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Product_BlogArticleProducts_BlogArticleProductArticleId",
                        column: x => x.BlogArticleProductArticleId,
                        principalTable: "BlogArticleProducts",
                        principalColumn: "article_id");
                    table.ForeignKey(
                        name: "FK_Product_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "CategoryID");
                    table.ForeignKey(
                        name: "FK_Product_Supplier_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Supplier",
                        principalColumn: "SupplierID");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTerritory",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    TerritoryId = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTerritory", x => new { x.EmployeeId, x.TerritoryId });
                    table.ForeignKey(
                        name: "FK_EmployeeTerritory_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeTerritory_Territory_TerritoryId",
                        column: x => x.TerritoryId,
                        principalTable: "Territory",
                        principalColumn: "TerritoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order Details",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "money", nullable: false),
                    Quantity = table.Column<short>(type: "smallint", nullable: false),
                    Discount = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order Details", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Order Details_Order_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order Details_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "CategoryName",
                table: "Category",
                column: "CategoryName");

            migrationBuilder.CreateIndex(
                name: "City",
                table: "Customer",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "CompanyName",
                table: "Customer",
                column: "CompanyName");

            migrationBuilder.CreateIndex(
                name: "PostalCode",
                table: "Customer",
                column: "PostalCode");

            migrationBuilder.CreateIndex(
                name: "Region",
                table: "Customer",
                column: "Region");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCustomerDemographic_CustomerTypeId",
                table: "CustomerCustomerDemographic",
                column: "CustomerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_ReportsTo",
                table: "Employee",
                column: "ReportsTo");

            migrationBuilder.CreateIndex(
                name: "LastName",
                table: "Employee",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "PostalCode1",
                table: "Employee",
                column: "PostalCode");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTerritory_TerritoryId",
                table: "EmployeeTerritory",
                column: "TerritoryId");

            migrationBuilder.CreateIndex(
                name: "CustomerID",
                table: "Order",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "CustomersOrders",
                table: "Order",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "EmployeeID",
                table: "Order",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "EmployeesOrders",
                table: "Order",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "OrderDate",
                table: "Order",
                column: "OrderDate");

            migrationBuilder.CreateIndex(
                name: "ShippedDate",
                table: "Order",
                column: "ShippedDate");

            migrationBuilder.CreateIndex(
                name: "ShippersOrders",
                table: "Order",
                column: "ShipVia");

            migrationBuilder.CreateIndex(
                name: "ShipPostalCode",
                table: "Order",
                column: "ShipPostalCode");

            migrationBuilder.CreateIndex(
                name: "OrderID",
                table: "Order Details",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "OrdersOrder_Details",
                table: "Order Details",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "ProductID",
                table: "Order Details",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "ProductsOrder_Details",
                table: "Order Details",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "CategoriesProducts",
                table: "Product",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "CategoryID",
                table: "Product",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_BlogArticleProductArticleId",
                table: "Product",
                column: "BlogArticleProductArticleId");

            migrationBuilder.CreateIndex(
                name: "ProductName",
                table: "Product",
                column: "ProductName");

            migrationBuilder.CreateIndex(
                name: "SupplierID",
                table: "Product",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "SuppliersProducts",
                table: "Product",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "CompanyName1",
                table: "Supplier",
                column: "CompanyName");

            migrationBuilder.CreateIndex(
                name: "PostalCode2",
                table: "Supplier",
                column: "PostalCode");

            migrationBuilder.CreateIndex(
                name: "IX_Territory_RegionID",
                table: "Territory",
                column: "RegionID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerCustomerDemographic");

            migrationBuilder.DropTable(
                name: "EmployeeTerritory");

            migrationBuilder.DropTable(
                name: "Order Details");

            migrationBuilder.DropTable(
                name: "CustomerDemographic");

            migrationBuilder.DropTable(
                name: "Territory");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Region");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Shipper");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Supplier");

            migrationBuilder.AddColumn<int>(
                name: "product_id",
                table: "BlogArticleProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
