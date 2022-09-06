# This is multitier web application:
- Application layer: NorthwindApiApp - REST API
- Presentation layer: NorthwindMvc - MVC client
- Data Access layers: 
1) ADO approach: Northwind.DataAccess.SqlServer
2) EF approach: Northwind.Services.EntityFrameworkCore (Database first), Northwind.Services.EntityFrameworkCore.Blogging (Code first)
- DataBases : NorthwindApiApp/Databases/Northwind.mdf & NorthwindBlogging.mdf (MSSQLLocalDB)
# Debug order:
1) Start NorthwindApiApp.exe;
2) Debug NorthwindMvc using IIS Express

![image](https://user-images.githubusercontent.com/62211469/188522458-54807699-54b9-448e-899d-e5f6b6e386ad.png)

![image](https://user-images.githubusercontent.com/62211469/188522499-714688fc-82a4-46bb-8839-5d5a839b076f.png)

![image](https://user-images.githubusercontent.com/62211469/188522565-abe5cd07-fd97-452c-86c5-38c6d3baa266.png)

![image](https://user-images.githubusercontent.com/62211469/188522638-fe9734ca-f821-45dc-b8ad-fcbd7296824f.png)

![image](https://user-images.githubusercontent.com/62211469/188522677-187121c5-4978-40cd-9a9a-19860b2da9ed.png)

# Solution diagram
![image](https://user-images.githubusercontent.com/62211469/188332130-0b8f8510-245c-40dd-ac9e-2f978a44a666.png)
