﻿using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.Employees;
using Northwind.Services.EntityFrameworkCore.Context;
using Northwind.Services.EntityFrameworkCore.Entities;
using AutoMapper;
using Northwind.Services.Interfaces;

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
            var entity = this.toEntitymapper.Map<EmployeeEntity>(employee);

            await this.context.Employees.AddAsync(entity);
            await this.context.SaveChangesAsync();

            return entity.EmployeeId;
        }

        public async Task<bool> DestroyEmployeeAsync(int employeeId)
        {
            var employee = this.context.Employees.FindAsync(employeeId).Result;

            if (employee is null)
            {
                return false;
            }

            var relatedOrders = this.context.Orders.Where(order => order.EmployeeId == employeeId);
            var relatedTerritories = this.context.EmployeeTerritories.Where(et => et.EmployeeId == employeeId);
            var relatedOrderDetails = from od in this.context.OrderDetails
                                      from ro in relatedOrders
                                      where od.OrderId == ro.OrderId
                                      select od;

            this.context.Orders.RemoveRange(relatedOrders);
            this.context.EmployeeTerritories.RemoveRange(relatedTerritories);
            this.context.OrderDetails.RemoveRange(relatedOrderDetails);
            this.context.Employees.Remove(employee);
            await this.context.SaveChangesAsync();
            return true;
        }

        public async IAsyncEnumerable<Employee> LookupEmployeesByLastNameAsync(ICollection<string> names)
        {
            var employees = this.context.Employees.AsAsyncEnumerable();

            await foreach (var e in employees)
            {
                if (names.Any(x =>
                {
                    if (x is null) return false;
                    return (e.FirstName + e.LastName).Contains(x, StringComparison.OrdinalIgnoreCase);
                }))
                {
                    yield return this.fromEntitymapper.Map<Employee>(e);
                }
            }
        }

        public async IAsyncEnumerable<Employee> GetEmployeesAsync(int offset, int limit)
        {
            var employees = this.context.Employees.OrderBy(x => x.EmployeeId).Skip(offset).Take(limit).AsAsyncEnumerable();

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

        public async Task<int> GetEmployeesCountAsync()
        {
            return await this.context.Employees.CountAsync();
        }
    }
}
