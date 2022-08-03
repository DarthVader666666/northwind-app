using System.Collections.Generic;

namespace Northwind.DataAccess.Employees
{
    /// <summary>
    /// Represents a DAO for Northwind employees.
    /// </summary>
    public interface IEmployeeDataAccessObject
    {
        int GetAmountOfEmployees();

        /// <summary>
        /// Inserts a new Northwind employee to a data storage.
        /// </summary>
        /// <param name="employee">A <see cref="EmployeeTransferObject"/>.</param>
        /// <returns>A data storage identifier of a new employee.</returns>
        int InsertEmployee(EmployeeTransferObject employee);

        /// <summary>
        /// Deletes a Northwind employee from a data storage.
        /// </summary>
        /// <param name="productId">An employee identifier.</param>
        /// <returns>True if a employee is deleted; otherwise false.</returns>
        bool DeleteEmployee(int productId);

        /// <summary>
        /// Updates a Northwind employee in a data storage.
        /// </summary>
        /// <param name="employee">A <see cref="EmployeeTransferObject"/>.</param>
        /// <returns>True if a employee is updated; otherwise false.</returns>
        bool UpdateEmployee(EmployeeTransferObject employee);

        /// <summary>
        /// Finds a Northwind employee using a specified identifier.
        /// </summary>
        /// <param name="productId">A data storage identifier of an existed employee.</param>
        /// <returns>A <see cref="EmployeeTransferObject"/> with specified identifier.</returns>
        EmployeeTransferObject FindEmployee(int productId);

        /// <summary>
        /// Selects products using specified offset and limit.
        /// </summary>
        /// <param name="offset">An offset of the first object.</param>
        /// <param name="limit">A limit of returned objects.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="EmployeeTransferObject"/>.</returns>
        IAsyncEnumerable<EmployeeTransferObject> SelectEmployees(int offset, int limit);

        /// <summary>
        /// Selects all Northwind products with specified names.
        /// </summary>
        /// <param name="productNames">A <see cref="IEnumerable{T}"/> of employee names.</param>
        /// <returns>A <see cref="IAsyncEnumerable{T}"/> of <see cref="EmployeeTransferObject"/>.</returns>
        IAsyncEnumerable<EmployeeTransferObject> SelectEmployeesByLastName(ICollection<string> productNames);
    }
}
