using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using System.Data.SqlClient;
using Northwind.DataAccess.Employees;
using Northwind.DataAccess.Products;

namespace Northwind.DataAccess.SqlServer
{
    /// <summary>
    /// Represents an abstract factory for creating Northwind DAO for SQL Server.
    /// </summary>
    public sealed class SqlServerDataAccessFactory : NorthwindDataAccessFactory
    {
        private readonly SqlConnection sqlConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerDataAccessFactory"/> class.
        /// </summary>
        /// <param name="sqlConnection">A database connection to SQL Server.</param>
        public SqlServerDataAccessFactory(string connectionString)
        {
            this.sqlConnection = new SqlConnection(connectionString) ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <inheritdoc/>
        public override IProductCategoryDataAccessObject GetProductCategoryDataAccessObject()
        {
            return new ProductCategorySqlServerDataAccessObject(this.sqlConnection);
        }

        /// <inheritdoc/>
        public override IProductDataAccessObject GetProductDataAccessObject()
        {
            return new ProductSqlServerDataAccessObject(this.sqlConnection);
        }

        /// <inheritdoc />
        public override IEmployeeDataAccessObject GetEmployeeDataAccessObject()
        {
            return new EmployeeSqlServerDataAccessObject(this.sqlConnection);
        }
    }
}
