using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using NorthwindApiApp;
using Xunit;

namespace Northwind.IntegrationTests
{
    public class EmplpoyeesTest
    {
        [Fact]
        public async Task Get_Employees()
        {
            using var application = new WebApplicationFactory<Startup>();
            using var client = application.CreateClient();

            var response = await client.GetAsync("/api/employees/total");

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var amountOfEmployees = await response.Content.ReadAsStringAsync();
            Assert.True(int.TryParse(amountOfEmployees, out _));
            Assert.True(int.Parse(amountOfEmployees) >= 0);
        }
    }
}
