using AutoMapper;
using Northwind.DataAccess;
using Northwind.DataAccess.Employees;
using Northwind.Services.Employees;

namespace Northwind.Services.DataAccess
{
    public class EmployeeManagementDataAccessService : IEmployeeManagementService
    {
        private readonly IEmployeeDataAccessObject employeeService;
        private readonly Mapper mapper;

        public EmployeeManagementDataAccessService(NorthwindDataAccessFactory factory)
        {
            this.employeeService = factory.GetEmployeeDataAccessObject();
            this.mapper = new Mapper(new MapperConfiguration(conf =>
            conf.CreateMap<EmployeeTransferObject, Employee>()));
        }

        public Task<int> CreateEmployeeAsync(Employee employee)
        {
            var mapper = new Mapper(new MapperConfiguration(conf =>
            conf.CreateMap<Employee, EmployeeTransferObject>()));

            var employeeTO = mapper.Map<EmployeeTransferObject>(employee);

            return Task.Run(() => this.employeeService.InsertEmployee(employeeTO));
        }

        public Task<bool> DestroyEmployeeAsync(int employeeId)
        {
            return Task.Run(() => this.employeeService.DeleteEmployee(employeeId));
        }

        public async IAsyncEnumerable<Employee> GetEmployeesAsync(int offset, int limit)
        {
            await foreach (var item in this.employeeService.SelectEmployees(offset, limit))
            {
                yield return this.mapper.Map<Employee>(item);
            }
        }

        public async IAsyncEnumerable<Employee> LookupEmployeesByLastNameAsync(ICollection<string> names)
        {
            await foreach (var item in this.employeeService.SelectEmployeesByLastName(names))
            {
                yield return this.mapper.Map<Employee>(item);
            }
        }

        public Task<(bool result, Employee employee)> TryGetEmployeeAsync(int employeeId)
        {
            return Task.Run(() =>
            {
                var product = this.employeeService.FindEmployee(employeeId);
                return (product is not null, this.mapper.Map<Employee>(product));
            });
        }

        public Task<bool> UpdateEmployeeAsync(int employeeId, Employee employee)
        {
            var mapper = new Mapper(new MapperConfiguration(conf =>
            conf.CreateMap<Employee, EmployeeTransferObject>()));

            return Task.Run(() =>
            {
                employee.EmployeeId = employeeId;
                var employeeTO = mapper.Map<EmployeeTransferObject>(employee);
                return this.employeeService.UpdateEmployee(employeeTO);
            });
        }
    }
}
