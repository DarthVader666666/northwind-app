using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Northwind.Services.Interfaces
{
    public interface IProductCategoryPicturesManagementService
    {
        /// <summary>
        /// Try to show a picture.
        /// </summary>
        /// <param name="id">An identifier.</param>
        /// <returns>True if exists; otherwise false.</returns>
        Task<(bool result, byte[] bytes)> TryGetCategoryPictureAsync(int id);

        /// <summary>
        /// Update a picture.
        /// </summary>
        /// <param name="id">An identifier.</param>
        /// <param name="stream">A <see cref="Stream"/>.</param>
        /// <returns>True if exists; otherwise false.</returns>
        Task<bool> UpdateCategoryPictureAsync(int id, Stream stream);

        /// <summary>
        /// Destroy a picture.
        /// </summary>
        /// <param name="id">An identifier.</param>
        /// <returns>True if exists; otherwise false.</returns>
        Task<bool> DestroyCategoryPictureAsync(int id);
    }
}
