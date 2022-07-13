using Microsoft.EntityFrameworkCore;
using Northwind.Services.Employees;
using Northwind.Services.EntityFrameworkCore.Context;
using Northwind.Services.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Services.EntityFrameworkCore
{
    /// <summary>
    /// Represents a management service for employees.
    /// </summary>
    public class EmployeeManagementService : IEmployeeManagementService
    {
        private readonly NorthwindContext context;

        public EmployeeManagementService(NorthwindContext context)
        {
            this.context = context;
        }

        public async Task<int> CreateEmployeeAsync(Employee employee)
        {
            var id = 1;

            if (this.context.Employees.AnyAsync().Result)
            {
                id = this.context.Employees.Max(x => x.EmployeeId);
                id++;
            }

            employee.EmployeeId = id;
            await this.context.Employees.AddAsync(employee);
            await this.context.SaveChangesAsync();

            return id;
        }

        public async Task<bool> DestroyEmployeeAsync(int employeeId)
        {
            var employee = this.context.Employees.FindAsync(employeeId).Result;

            if (employee is null)
            {
                return false;
            }

            this.context.Employees.Remove(employee);
            await this.context.SaveChangesAsync();
            return true;
        }

        public IAsyncEnumerable<Employee> LookupEmployeesByLastNameAsync(ICollection<string> names)
        {
            var employees = this.context.Employees.Where(e => names.Any(n => n == e.LastName));

            if (!employees.Any())
            {
                return null;
            }

            return employees.AsAsyncEnumerable();
        }

        public IAsyncEnumerable<Employee> GetEmployeesAsync(int offset, int limit) =>
            this.context.Employees.Skip(offset).Take(limit).AsAsyncEnumerable();

        public async Task<(bool, Employee)> TryGetEmployeeAsync(int employeeId)
        {
            var employee = await Task.Run(async () =>
            await this.context.Employees.FindAsync(employeeId));

            return (employee is not null, employee);
        }

        public async Task<bool> UpdateEmployeeAsync(int employeeId, Employee employee)
        {
            if (employee.EmployeeId != employeeId)
            {
                return false;
            }

            this.context.Entry(employee).State = EntityState.Modified;
            await this.context.SaveChangesAsync();

            return true;
        }
    }
}


