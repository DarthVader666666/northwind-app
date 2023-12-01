using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        private static byte[] OleHeader = new byte[] { 21, 28, 47, 0, 2, 0, 0, 0, 13, 0, 14, 0, 20,
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

            if (count == 0)
            {
                return this.View(new EmployeesListModel());
            }

            var json = await this.httpClient.GetStringAsync(
                BasePath + $"?offset={(page - 1) * PageSize}&limit={PageSize}");

            var employees = JsonConvert.DeserializeObject<List<Employee>>(json);

            var viewModel = new EmployeesListModel()
            {
                Employees = employees.
                Select(x =>
                {
                    var e = this.mapper.Map<Employee, EmployeeModel>(x);
                    e.Photo = ConvertNorthwindPhoto(e.Photo);
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

        [HttpGet]
        public async Task<IActionResult> DeleteAsync(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var json = await (await this.httpClient.GetAsync(BasePath + "/" + id)).Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<EmployeeModel>(json);

            if (obj == null)
            {
                return this.NotFound();
            }

            return this.View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePostAsync(int? id)
        {
            var json = await (await this.httpClient.GetAsync(BasePath + "/" + id)).Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<EmployeeModel>(json);

            if (obj == null)
            {
                return this.NotFound();
            }

            await this.httpClient.DeleteAsync(BasePath + "/" + id);
            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateAsync(int id)
        {
            var json = await this.httpClient.GetStringAsync(BasePath + "/" + id);
            var employee = JsonConvert.DeserializeObject<Employee>(json);
            var employeeModel = this.mapper.Map<Employee, EmployeeModel>(employee);
            employeeModel.Photo = ConvertNorthwindPhoto(employee.Photo);

            employeeModel.PhotoStringValue = BitConverter.ToString(employeeModel.Photo)!;

            return this.View(employeeModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAsync(EmployeeModel employeeModel)
        {
            var employee = this.mapper.Map<EmployeeModel, Employee>(employeeModel);

            if (employeeModel.PictureForm is not null)
            {
                using var reader = new BinaryReader(employeeModel.PictureForm.OpenReadStream());
                employee.Photo = reader.ReadBytes((int)employeeModel.PictureForm.Length);
            }
            else
            {
                var hex = employeeModel.PhotoStringValue is not null ?
                    new string(employeeModel.PhotoStringValue.Where(x => x != '-').ToArray()) : null;

                employee.Photo = hex is not null ? Convert.FromHexString(hex) : null;
            }

            var json = JsonConvert.SerializeObject(employee);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            await this.httpClient.PutAsync(BasePath + "/" + employeeModel.EmployeeId, data);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> FindByNameAsync(EmployeesListModel model)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, this.httpClient.BaseAddress +
                "api/employees/by_name");

            var json = JsonConvert.SerializeObject(new string[] { model.PagingInfo.Name });
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await this.httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return this.View(new List<EmployeeModel>());
            }

            json = await response.Content.ReadAsStringAsync();

            var employees = JsonConvert.DeserializeObject<List<Employee>>(json);

            var viewModel = employees is null ?
                new List<EmployeeModel>() :
                employees.Select(x =>
                {
                    x.Photo = x.Photo.HasHeader(OleHeader) ? x.Photo[OleHeader.Length..] : x.Photo;
                    return this.mapper.Map<Employee, EmployeeModel>(x);
                });

            return this.View(viewModel);
        }


        private static byte[] ConvertNorthwindPhoto(byte[] source) =>
            source.HasHeader(OleHeader) ? source[OleHeader.Length..] : source;
    }
}
