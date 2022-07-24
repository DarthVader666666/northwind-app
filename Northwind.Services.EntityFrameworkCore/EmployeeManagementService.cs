﻿using Microsoft.EntityFrameworkCore;
using Northwind.Services.Employees;
using Northwind.Services.EntityFrameworkCore.Context;
using Northwind.Services.EntityFrameworkCore.Entities;
using AutoMapper;

namespace Northwind.Services.EntityFrameworkCore
{
    /// <summary>
    /// Represents a management service for employees.
    /// </summary>
    public class EmployeeManagementService : IEmployeeManagementService
    {
        private readonly NorthwindContext context;
        private readonly IMapper toEntitymapper;
        private readonly IMapper fromEntitymapper;

        public EmployeeManagementService(NorthwindContext context)
        {
            this.context = context;
            this.toEntitymapper = new Mapper(new MapperConfiguration(conf =>
            conf.CreateMap<Employee, EmployeeEntity>()));
            this.fromEntitymapper = new Mapper(new MapperConfiguration(conf =>
            conf.CreateMap<EmployeeEntity, Employee>()));
        }

        public async Task<int> CreateEmployeeAsync(Employee employee)
        {
            var id = 1;

            if (this.context.Employees.AnyAsync().Result)
            {
                id = this.context.Employees.Max(x => x.EmployeeId);
                id++;
            }

            var entity = this.toEntitymapper.Map<EmployeeEntity>(employee);

            entity.EmployeeId = id;
            await this.context.Employees.AddAsync(entity);
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

        public async IAsyncEnumerable<Employee> LookupEmployeesByLastNameAsync(ICollection<string> names)
        {
            var employees = this.context.Employees.Where(e => names.Any(n => n == e.LastName)).AsAsyncEnumerable();

            await foreach(var item in employees)
            {
                yield return this.fromEntitymapper.Map<Employee>(item);
            }
        }

        public async IAsyncEnumerable<Employee> GetEmployeesAsync(int offset, int limit)
        {
            var employees = this.context.Employees.Skip(offset).Take(limit).AsAsyncEnumerable();

            await foreach (var item in employees)
            {
                yield return this.fromEntitymapper.Map<Employee>(item);
            }
        }

        public async Task<(bool, Employee)> TryGetEmployeeAsync(int employeeId)
        {
            var employee = await this.context.Employees.FindAsync(employeeId);

            return (employee is not null, this.fromEntitymapper.Map<Employee>(employee));
        }

        public async Task<bool> UpdateEmployeeAsync(int employeeId, Employee employee)
        {
            if (employee.EmployeeId != employeeId)
            {
                return false;
            }

            this.context.Entry(this.toEntitymapper.Map<EmployeeEntity>(employee)).State = EntityState.Modified;
            await this.context.SaveChangesAsync();

            return true;
        }
    }
}