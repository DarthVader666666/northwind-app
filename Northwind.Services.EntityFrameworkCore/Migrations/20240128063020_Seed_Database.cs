using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Northwind.Services.EntityFrameworkCore.Migrations
{
    public partial class Seed_Database : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;

            string script = environment switch
            {
                "Docker" => File.ReadAllText("../src/NorthwindApiApp/Databases/DB_Create_Script.sql"),
                "Development" => File.ReadAllText("../NorthwindApiApp/Databases/DB_Create_Script.sql"),
                _ => throw new Exception("Wrong Environment!")
            };

            migrationBuilder.Sql(script);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
