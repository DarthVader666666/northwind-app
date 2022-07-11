using System.Collections.Generic;
using System.Threading.Tasks;
using Northwind.Services.Entities;

namespace Northwind.Services.Employees
{
    /// <summary>
    /// Represents a management service for employees.
    /// </summary>
    public interface IEmployeeManagementService
    {
        /// <summary>
        /// Shows a list of products using specified offset and limit for pagination.
        /// </summary>
        /// <param name="offset">An offset of the first element to return.</param>
        /// <param name="limit">A limit of elements to return.</param>
        /// <returns>A <see cref="IList{T}"/> of <see cref="Employee"/>.</returns>
        IAsyncEnumerable<Employee> GetEmployeesAsync(int offset, int limit);

        /// <summary>
        /// Try to show a employee with specified identifier.
        /// </summary>
        /// <param name="employeeId">A employee identifier.</param>
        /// <returns>Returns true if a employee is returned; otherwise false.</returns>
        Task<(bool result, Employee employee)> TryGetEmployeeAsync(int employeeId);

        /// <summary>
        /// Creates a new employee.
        /// </summary>
        /// <param name="employee">A <see cref="Employee"/> to create.</param>
        /// <returns>An identifier of a created employee.</returns>
        Task<int> CreateEmployeeAsync(Employee employee);

        /// <summary>
        /// Destroys an existed employee.
        /// </summary>
        /// <param name="employeeId">A employee identifier.</param>
        /// <returns>True if a employee is destroyed; otherwise false.</returns>
        Task<bool> DestroyEmployeeAsync(int employeeId);

        /// <summary>
        /// Updates a employee.
        /// </summary>
        /// <param name="employeeId">A employee identifier.</param>
        /// <param name="employee">A <see cref="Employee"/>.</param>
        /// <returns>True if a employee is updated; otherwise false.</returns>
        Task<bool> UpdateEmployeeAsync(int employeeId, Employee employee);

        /// <summary>
        /// Looks up for employee with specified names.
        /// </summary>
        /// <param name="names">A list of employees names.</param>
        /// <returns>A list of employees with specified names.</returns>
        IAsyncEnumerable<Employee> LookupEmployeesByLastNameAsync(ICollection<string> names);
    }
}
