using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Northwind.DataAccess.Employees;

namespace Northwind.DataAccess.SqlServer
{
    /// <summary>
    /// Represents a SQL Server-tailored DAO for Northwind products.
    /// </summary>
    public sealed class EmployeeSqlServerDataAccessObject : IEmployeeDataAccessObject, IDisposable
    {
        private readonly SqlConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeSqlServerDataAccessObject"/> class.
        /// </summary>
        /// <param name="connection">A <see cref="SqlConnection"/>.</param>
        public EmployeeSqlServerDataAccessObject(SqlConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
            this.connection.Close();
            this.connection.Open();
        }

        public int GetAmountOfEmployees()
        {
            using (var command = new SqlCommand
            {
                CommandType = CommandType.Text,
                CommandText = "SELECT * FROM dbo.Employees",
                Connection = this.connection,
            })
            {
                var reader = command.ExecuteReader();
                var count = 0;

                while (reader.Read())
                {
                    count++;
                }

                return count;
            }
        }

        /// <inheritdoc/>
        public int InsertEmployee(EmployeeTransferObject employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            using (var command = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "InsertEmployee",
                Connection = this.connection
            })
            {
                AddSqlParameters(employee, command);
                command.ExecuteNonQuery();
                return (int)command.Parameters["@employeeId"].Value;
            }
        }

        /// <inheritdoc/>
        public bool DeleteEmployee(int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentException("Must be greater than zero.", nameof(employeeId));
            }

            using (var command = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "DeleteEmployee",
                Connection = this.connection
            })
            {
                const string employeeIdParameter = "@employeeID";
                command.Parameters.Add(employeeIdParameter, SqlDbType.Int);
                command.Parameters[employeeIdParameter].Value = employeeId;

                var result = command.ExecuteNonQuery();
                return employeeId > 0;
            }
        }

        /// <inheritdoc/>
        public EmployeeTransferObject FindEmployee(int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentException("Must be greater than zero.", nameof(employeeId));
            }

            using (var command = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "FindEmployee",
                Connection = this.connection
            })
            {
                const string employeeIdParameter = "@employeeId";
                command.Parameters.Add(employeeIdParameter, SqlDbType.Int);
                command.Parameters[employeeIdParameter].Value = employeeId;

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }

                    return CreateEmployee(reader);
                }
            }
        }

        /// <inheritdoc />
        public IAsyncEnumerable<EmployeeTransferObject> SelectEmployees(int offset, int limit)
        {
            if (offset < 0)
            {
                throw new ArgumentException("Must be greater than zero or equals zero.", nameof(offset));
            }

            if (limit < 1)
            {
                throw new ArgumentException("Must be greater than zero.", nameof(limit));
            }

            return this.ExecuteReaderAsync("SelectEmployees", ("offset", offset), ("limit", limit));
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<EmployeeTransferObject> SelectEmployeesByLastName(ICollection<string> names)
        {
            if (names == null)
            {
                throw new ArgumentNullException(nameof(names));
            }

            if (names.Count < 1)
            {
                throw new ArgumentException("Collection is empty.", nameof(names));
            }

            const string commandTemplate =
            @"SELECT p.EmployeeID, p.LastName, p.FirstName, p.Title, p.TitleOfCourtesy, p.BirthDate,
            p.HireDate, p.Address, p.City, p.Region, p.PostalCode, p.Country, p.HomePhone, p.Extension,
            p.Photo, p.Notes, p.ReportsTo, p.PhotoPath
            FROM dbo.Employees as p
            WHERE p.LastName in ('{0}')
            ORDER BY p.EmployeeID";

            string commandText = string.Format(CultureInfo.CurrentCulture, commandTemplate, string.Join("', '", names));

            using (var command = new SqlCommand 
            { 
                CommandText = commandText,
                Connection = this.connection,
                CommandType = CommandType.Text
            })
            {
                var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    yield return CreateEmployee(reader);
                }
            }
        }

        /// <inheritdoc/>
        public bool UpdateEmployee(EmployeeTransferObject employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            using (var command = new SqlCommand 
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "UpdateEmployee",
                Connection = this.connection
            })
            {
                AddSqlParameters(employee, command);
                const string employeeId = "@employeeId";
                command.Parameters.Add(employeeId, SqlDbType.Int);
                command.Parameters[employeeId].Value = employee.EmployeeId;

                return command.ExecuteNonQuery() > 0;
            }
        }

        private static EmployeeTransferObject CreateEmployee(SqlDataReader reader)
        {
            var id = (int)reader["EmployeeID"];
            var lastName = (string)reader["LastName"];

            const string firstNameColumnName = "FirstName";
            string? firstName;

            if (reader[firstNameColumnName] != DBNull.Value)
            {
                firstName = (string)reader[firstNameColumnName];
            }
            else
            {
                firstName = null;
            }

            const string titleColumnName = "Title";
            string? title;

            if (reader[titleColumnName] != DBNull.Value)
            {
                title = (string)reader[titleColumnName];
            }
            else
            {
                title = null;
            }

            const string titleOfCourtesyColumnName = "TitleOfCourtesy";
            string titleOfCourtesy;

            if (reader[titleOfCourtesyColumnName] != DBNull.Value)
            {
                titleOfCourtesy = (string)reader[titleOfCourtesyColumnName];
            }
            else
            {
                titleOfCourtesy = null;
            }

            const string birthDateColumnName = "BirthDate";
            DateTime? birthDate;

            if (reader[birthDateColumnName] != DBNull.Value)
            {
                birthDate = (DateTime)reader[birthDateColumnName];
            }
            else
            {
                birthDate = null;
            }

            const string hireDateColumnName = "HireDate";
            DateTime? hireDate;

            if (reader[hireDateColumnName] != DBNull.Value)
            {
                hireDate = (DateTime)reader[hireDateColumnName];
            }
            else
            {
                hireDate = null;
            }

            const string AddressColumnName = "Address";
            string? address;

            if (reader[AddressColumnName] != DBNull.Value)
            {
                address = (string)reader[AddressColumnName];
            }
            else
            {
                address = null;
            }

            const string cityColumnName = "City";
            string? city;

            if (reader[cityColumnName] != DBNull.Value)
            {
                city = (string)reader[cityColumnName];
            }
            else
            {
                city = null;
            }

            const string regionColumnName = "Region";
            string region;

            if (reader[regionColumnName] != DBNull.Value)
            {
                region = (string)reader[regionColumnName];
            }
            else
            {
                region = null;
            }

            const string postalCodeColumnName = "PostalCode";
            string postalCode;

            if (reader[regionColumnName] != DBNull.Value)
            {
                postalCode = (string)reader[postalCodeColumnName];
            }
            else
            {
                postalCode = null;
            }

            const string countryCodeColumnName = "Country";
            string country;

            if (reader[countryCodeColumnName] != DBNull.Value)
            {
                country = (string)reader[countryCodeColumnName];
            }
            else
            {
                country = null;
            }

            const string homePhoneColumnName = "HomePhone";
            string homePhone;

            if (reader[homePhoneColumnName] != DBNull.Value)
            {
                homePhone = (string)reader[homePhoneColumnName];
            }
            else
            {
                homePhone = null;
            }

            const string extensionColumnName = "Extension";
            string extension;

            if (reader[extensionColumnName] != DBNull.Value)
            {
                extension = (string)reader[extensionColumnName];
            }
            else
            {
                extension = null;
            }

            const string photoColumnName = "Photo";
            byte[] photo;

            if (reader[photoColumnName] != DBNull.Value)
            {
                photo = (byte[])reader[photoColumnName];
            }
            else
            {
                photo = null;
            }

            const string notesColumnName = "Notes";
            string notes;

            if (reader[notesColumnName] != DBNull.Value)
            {
                notes = (string)reader[notesColumnName];
            }
            else
            {
                notes = null;
            }

            const string reportsToColumnName = "ReportsTo";
            int? reportsTo;

            if (reader[reportsToColumnName] != DBNull.Value)
            {
                reportsTo = (int)reader[reportsToColumnName];
            }
            else
            {
                reportsTo = null;
            }

            const string photoPathColumnName = "PhotoPath";
            string? photoPath;

            if (reader[photoPathColumnName] != DBNull.Value)
            {
                photoPath = (string)reader[photoPathColumnName];
            }
            else
            {
                photoPath = null;
            }

            return new EmployeeTransferObject
            {
                EmployeeId = id,
                LastName = lastName,
                FirstName = firstName,
                Title = title,
                TitleOfCourtesy = titleOfCourtesy,
                BirthDate = birthDate,
                HireDate = hireDate,
                Address = address,
                City = city,
                PostalCode = postalCode,
                Country = country,
                HomePhone = homePhone,
                Extension = extension,
                Photo = photo,
                Notes = notes,
                ReportsTo = reportsTo,
                PhotoPath = photoPath,
            };
        }

        private static void AddSqlParameters(EmployeeTransferObject employee, SqlCommand command)
        {
            const string IdParameter = "@employeeId";
            command.Parameters.Add(IdParameter, SqlDbType.Int, 4);
            command.Parameters[IdParameter].Direction = ParameterDirection.Output;

            const string lastNameParameter = "@lastName";
            command.Parameters.Add(lastNameParameter, SqlDbType.NVarChar, 20);
            command.Parameters[lastNameParameter].Value = employee.LastName;

            const string firstNameParameter = "@firstName";
            command.Parameters.Add(firstNameParameter, SqlDbType.NVarChar, 10);
            command.Parameters[firstNameParameter].IsNullable = false;

            if (employee.FirstName != null)
            {
                command.Parameters[firstNameParameter].Value = employee.FirstName;
            }
            else
            {
                command.Parameters[firstNameParameter].Value = DBNull.Value;
            }

            const string titleParameter = "@title";
            command.Parameters.Add(titleParameter, SqlDbType.NVarChar, 30);
            command.Parameters[titleParameter].IsNullable = true;

            if (employee.Title != null)
            {
                command.Parameters[titleParameter].Value = employee.Title;
            }
            else
            {
                command.Parameters[titleParameter].Value = DBNull.Value;
            }

            const string titleOfCourtesyParameter = "@titleOfCourtesy";
            command.Parameters.Add(titleOfCourtesyParameter, SqlDbType.NVarChar, 25);
            command.Parameters[titleOfCourtesyParameter].IsNullable = true;

            if (employee.TitleOfCourtesy != null)
            {
                command.Parameters[titleOfCourtesyParameter].Value = employee.TitleOfCourtesy;
            }
            else
            {
                command.Parameters[titleOfCourtesyParameter].Value = DBNull.Value;
            }

            const string birthDateParameter = "@birthDate";
            command.Parameters.Add(birthDateParameter, SqlDbType.DateTime2);
            command.Parameters[birthDateParameter].IsNullable = true;

            if (employee.BirthDate != null)
            {
                command.Parameters[birthDateParameter].Value = employee.BirthDate;
            }
            else
            {
                command.Parameters[birthDateParameter].Value = DBNull.Value;
            }

            const string hireDateParameter = "@hireDate";
            command.Parameters.Add(hireDateParameter, SqlDbType.DateTime2);
            command.Parameters[hireDateParameter].IsNullable = true;

            if (employee.HireDate != null)
            {
                command.Parameters[hireDateParameter].Value = employee.HireDate;
            }
            else
            {
                command.Parameters[hireDateParameter].Value = DBNull.Value;
            }

            const string addressParameter = "@address";
            command.Parameters.Add(addressParameter, SqlDbType.NVarChar, 60);
            command.Parameters[addressParameter].IsNullable = true;

            if (employee.Address != null)
            {
                command.Parameters[addressParameter].Value = employee.Address;
            }
            else
            {
                command.Parameters[addressParameter].Value = DBNull.Value;
            }

            const string cityParameter = "@city";
            command.Parameters.Add(cityParameter, SqlDbType.NVarChar, 15);
            command.Parameters[cityParameter].IsNullable = true;

            if (employee.City != null)
            {
                command.Parameters[cityParameter].Value = employee.City;
            }
            else
            {
                command.Parameters[cityParameter].Value = DBNull.Value;
            }

            const string regionParameter = "@region";
            command.Parameters.Add(regionParameter, SqlDbType.NVarChar, 15);
            command.Parameters[regionParameter].IsNullable = true;

            if (employee.Region != null)
            {
                command.Parameters[regionParameter].Value = employee.Region;
            }
            else
            {
                command.Parameters[regionParameter].Value = DBNull.Value;
            }

            const string postalCodeParameter = "@postalCode";
            command.Parameters.Add(postalCodeParameter, SqlDbType.NVarChar, 10);
            command.Parameters[postalCodeParameter].IsNullable = true;

            if (employee.PostalCode != null)
            {
                command.Parameters[postalCodeParameter].Value = employee.PostalCode;
            }
            else
            {
                command.Parameters[postalCodeParameter].Value = DBNull.Value;
            }

            const string countryParameter = "@country";
            command.Parameters.Add(countryParameter, SqlDbType.NVarChar, 15);
            command.Parameters[countryParameter].IsNullable = true;

            if (employee.Country != null)
            {
                command.Parameters[countryParameter].Value = employee.Country;
            }
            else
            {
                command.Parameters[countryParameter].Value = DBNull.Value;
            }

            const string homePhoneParameter = "@homePhone";
            command.Parameters.Add(homePhoneParameter, SqlDbType.NVarChar, 24);
            command.Parameters[homePhoneParameter].IsNullable = true;

            if (employee.HomePhone != null)
            {
                command.Parameters[homePhoneParameter].Value = employee.HomePhone;
            }
            else
            {
                command.Parameters[homePhoneParameter].Value = DBNull.Value;
            }

            const string extensionParameter = "@extension";
            command.Parameters.Add(extensionParameter, SqlDbType.NVarChar, 4);
            command.Parameters[extensionParameter].IsNullable = true;

            if (employee.Extension != null)
            {
                command.Parameters[extensionParameter].Value = employee.Extension;
            }
            else
            {
                command.Parameters[extensionParameter].Value = DBNull.Value;
            }

            const string photoParameter = "@photo";
            command.Parameters.Add(photoParameter, SqlDbType.Image);
            command.Parameters[photoParameter].IsNullable = true;

            if (employee.Photo != null)
            {
                command.Parameters[photoParameter].Value = employee.Photo;
            }
            else
            {
                command.Parameters[photoParameter].Value = DBNull.Value;
            }

            const string notesParameter = "@notes";
            command.Parameters.Add(notesParameter, SqlDbType.NText);
            command.Parameters[notesParameter].IsNullable = true;

            if (employee.Notes != null)
            {
                command.Parameters[notesParameter].Value = employee.Notes;
            }
            else
            {
                command.Parameters[notesParameter].Value = DBNull.Value;
            }

            const string reportsToParameter = "@reportsTo";
            command.Parameters.Add(reportsToParameter, SqlDbType.Int);
            command.Parameters[reportsToParameter].IsNullable = true;

            if (employee.ReportsTo != null && (employee.ReportsTo > 0 && employee.ReportsTo < 10))
            {
                command.Parameters[reportsToParameter].Value = employee.ReportsTo;
            }
            else
            {
                command.Parameters[reportsToParameter].Value = DBNull.Value;
            }

            const string photoPathParameter = "@photoPath";
            command.Parameters.Add(photoPathParameter, SqlDbType.NVarChar, 255);
            command.Parameters[photoPathParameter].IsNullable = true;

            if (employee.PhotoPath != null)
            {
                command.Parameters[photoPathParameter].Value = employee.PhotoPath;
            }
            else
            {
                command.Parameters[photoPathParameter].Value = DBNull.Value;
            }
        }

        private async IAsyncEnumerable<EmployeeTransferObject> ExecuteReaderAsync(
            string commandText, params (string key, object value)[] parameters)
        {
            using (var command = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = commandText,
                Connection = this.connection
            })
            {
                foreach (var item in parameters)
                {
                    command.Parameters.Add(new SqlParameter(item.key, item.value));
                }

                var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    yield return CreateEmployee(reader);
                }
            }
        }

        public void Dispose()
        {
            this.connection.Close();
        }
    }
}
