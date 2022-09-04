# This is multitier web application:
# Application layer: NorthwindApiApp - REST API
# Presentation layer: NorthwindMvc - MVC client
# Data Access layers: 
# 1) ADO approach: Northwind.DataAccess.SqlServer
# 2) EF approach: Northwind.Services.EntityFrameworkCore (Database first), Northwind.Services.EntityFrameworkCore.Blogging (Code first)
# DataBases : NorthwindApiApp/Databases/Northwind.mdf & NorthwindBlogging.mdf (MSSQLLocalDB)
# Debug order:
# 1) Start NorthwindApiApp.exe;
# 2) Debug NorthwindMvc using IIS Express
