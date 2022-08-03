using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Northwind.Services.Employees;
using NorthwindMvc.Extensions;
using NorthwindMvc.Models;
using NorthwindMvc.Models.EmployeeModels;

namespace NorthwindMvc.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IMapper mapper;
        private const int PageSize = 7;
        private const string BasePath = "/api/employees";
        private readonly HttpClient httpClient;
        private readonly byte[] OleHeader = new byte[] { 21, 28, 47, 0, 2, 0, 0, 0, 13, 0, 14, 0, 20,
            0, 33, 0, 255, 255, 255, 255, 66, 105, 116, 109, 97, 112, 32, 73, 109, 97, 103, 101, 0, 80, 97,
            105, 110, 116, 46, 80, 105, 99, 116, 117, 114, 101, 0, 1, 5, 0, 0, 2, 0, 0, 0, 7, 0, 0, 0, 80,
            66, 114, 117, 115, 104, 0, 0, 0, 0, 0, 0, 0, 0, 0, 32, 84, 0, 0 };

        public EmployeeController(HttpClient client, IMapper mapper)
        {
            this.httpClient = client ?? throw new ArgumentNullException();
            this.mapper = mapper ?? throw new ArgumentNullException();
        }

        // GET: EmployeeController
        public async Task<ActionResult> IndexAsync(int page = 1)
        {
            var count = int.Parse(await this.httpClient.GetStringAsync(BasePath + "/total"));

            var json = await this.httpClient.GetStringAsync(
                BasePath + $"?offset={(page - 1) * PageSize}&limit={PageSize}");

            var employees = JsonConvert.DeserializeObject<List<Employee>>(json);

            var viewModel = new EmployeesListModel()
            {
                Employees = employees.
                Select(x =>
                {
                    var e = this.mapper.Map<Employee, EmployeeModel>(x);
                    e.Photo = e.Photo.HasHeader(this.OleHeader) ? e.Photo[this.OleHeader.Length..] : e.Photo;
                    return e;
                }),
                PagingInfo = new PagingInfo 
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = count
                }
            };

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(EmployeeModel employeeModel)
        {
            var employee = this.mapper.Map<Employee>(employeeModel);

            using var reader = new BinaryReader(employeeModel.PictureForm.OpenReadStream());
            employee.Photo = reader.ReadBytes((int)employeeModel.PictureForm.Length);

            var json = JsonConvert.SerializeObject(employee);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            await this.httpClient.PostAsync(BasePath, data);

            return this.RedirectToAction("Index");
        }
    }
}
