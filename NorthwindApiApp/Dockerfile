#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NorthwindApiApp/NorthwindApiApp.csproj", "NorthwindApiApp/"]
COPY ["Northwind.DataAccess.SqlServer/Northwind.DataAccess.SqlServer.csproj", "Northwind.DataAccess.SqlServer/"]
COPY ["Northwind.DataAccess/Northwind.DataAccess.csproj", "Northwind.DataAccess/"]
COPY ["Northwind.Services.DataAccess/Northwind.Services.DataAccess.csproj", "Northwind.Services.DataAccess/"]
COPY ["Northwind.Services/Northwind.Services.csproj", "Northwind.Services/"]
COPY ["Northwind.Services.EntityFrameworkCore.Blogging/Northwind.Services.EntityFrameworkCore.Blogging.csproj", "Northwind.Services.EntityFrameworkCore.Blogging/"]
COPY ["Northwind.Services.EntityFrameworkCore/Northwind.Services.EntityFrameworkCore.csproj", "Northwind.Services.EntityFrameworkCore/"]
RUN dotnet restore "NorthwindApiApp/NorthwindApiApp.csproj"
COPY . .
WORKDIR "/src/NorthwindApiApp"
RUN dotnet build "NorthwindApiApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NorthwindApiApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NorthwindApiApp.dll"]