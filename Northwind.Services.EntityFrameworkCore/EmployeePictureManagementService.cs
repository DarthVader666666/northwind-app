using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using Northwind.Services.EntityFrameworkCore.Context;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.Interfaces;

namespace Northwind.Services.DataAccess
{
    public class EmployeePictureManagementService : IEmployeePicturesManagementService
    {
        private readonly NorthwindContext context;

        public EmployeePictureManagementService(NorthwindContext context)
        {
            this.context = context;
        }

        public async Task<bool> DestroyEmployeePictureAsync(int EmployeeId)
        {
            var employee = await this.context.Employees.FindAsync(EmployeeId);

            if (employee is null)
            {
                return false;
            }

            employee.Photo = null;
            await this.context.SaveChangesAsync();
            return true;
        }

        public async Task<(bool, byte[])> TryGetEmployeePictureAsync(int EmployeeId)
        {
            var employee = await this.context.Employees.FindAsync(EmployeeId);

            return (employee.Photo is not null, employee.Photo);
        }

        public async Task<bool> UpdateEmployeePictureAsync(int EmployeeId, Stream stream)
        {
            var employee = await this.context.Employees.FindAsync(EmployeeId);

            if (employee is null)
            {
                return false;
            }

            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            employee.Photo = memoryStream.ToArray();

            this.context.Entry(employee).State = EntityState.Modified;
            await this.context.SaveChangesAsync();

            return employee.Photo.Length > 0;
        }
    }
}

