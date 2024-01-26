using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Northwind.Services.Interfaces
{
    public interface IEmployeePicturesManagementService
    {
        /// <summary>
        /// Try to show a picture.
        /// </summary>
        /// <param name="id">An identifier.</param>
        /// <returns>True if exists; otherwise false.</returns>
        Task<(bool result, byte[] bytes)> TryGetEmployeePictureAsync(int id);

        /// <summary>
        /// Update a picture.
        /// </summary>
        /// <param name="id">An identifier.</param>
        /// <param name="stream">A <see cref="Stream"/>.</param>
        /// <returns>True if exists; otherwise false.</returns>
        Task<bool> UpdateEmployeePictureAsync(int id, Stream stream);

        /// <summary>
        /// Destroy a picture.
        /// </summary>
        /// <param name="id">An identifier.</param>
        /// <returns>True if exists; otherwise false.</returns>
        Task<bool> DestroyEmployeePictureAsync(int id);
    }
}
