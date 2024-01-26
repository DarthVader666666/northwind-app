using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Employees;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Northwind.Services.Interfaces;
using Microsoft.AspNetCore.Cors;

namespace NorthwindApiApp.Controllers
{
    [Route("api/[controller]")]
    [EnableCors]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeManagementService service;
        private readonly IEmployeePicturesManagementService pictureService;

        public EmployeesController(IEmployeeManagementService service, IEmployeePicturesManagementService pictureService)
        {
            this.service = service;
            this.pictureService = pictureService;
        }

        [HttpGet("total")]
        public async Task<ActionResult<int>> ReadAmountOfEmployees()
        {
            return await this.service.GetEmployeesCountAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employee>> ReadEmployee(int id)
        {
            var task = await this.service.TryGetEmployeeAsync(id);

            return task.result ? this.Ok(task.employee) : this.NotFound();
        }

        [HttpGet]
        public ActionResult<IAsyncEnumerable<Employee>> ReadEmployees(
            [FromQuery(Name = "offset")]int offset, [FromQuery(Name = "limit")]int limit)
        {
            var employees = this.service.GetEmployeesAsync(offset, limit);
            return employees is not null ? this.Ok(employees) : this.BadRequest();
        }

        [HttpGet("by_name")]
        public ActionResult<IAsyncEnumerable<Employee>> ReadEmployees(List<string> names)
        {
            if (names is null || !names.Any())
            {
                return this.BadRequest();
            }

            var result = this.service.LookupEmployeesByLastNameAsync(names);

            if (result is null)
            {
                return this.NotFound("No employees with such names found.");
            }

            return this.Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            if (employee is null)
            {
                return this.BadRequest();
            }

            var id = await this.service.CreateEmployeeAsync(employee);
            return employee is not null ? this.Ok($"New employee created Id = {id}") : this.BadRequest();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
        {
            var result = await this.service.UpdateEmployeeAsync(id, employee);
            return result ? this.NoContent() : this.NotFound($"employee with id = {id} not found.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (id <= 0)
            {
                return this.NotFound($"employee Id = {id} NotFound.");
            }

            var result = await this.service.DestroyEmployeeAsync(id);

            return result ? this.NoContent() : this.NotFound($"employee Id = {id} NotFound.");
        }

        [HttpPut("{id:int}/picture")]
        public async Task<IActionResult> UpdateEmployeeImage(int id)
        {
            var stream = this.HttpContext.Request.Form.Files[0].OpenReadStream();
            var result = await this.pictureService.UpdateEmployeePictureAsync(id, stream);
            stream.Close();

            if (result)
            {
                return this.NoContent();
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpDelete("{id:int}/picture")]
        public async Task<IActionResult> DeleteEmployeeImage(int id)
        {
            var result = await this.pictureService.DestroyEmployeePictureAsync(id);

            if (result)
            {
                return this.NoContent();
            }
            else
            {
                return this.NotFound();
            }
        }
    }
}
