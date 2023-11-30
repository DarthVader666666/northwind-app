using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Northwind.Services
{
    public interface IProductCategoryPicturesManagementService
    {
        /// <summary>
        /// Try to show a product category picture.
        /// </summary>
        /// <param name="categoryId">A product category identifier.</param>
        /// <returns>True if a product category is exist; otherwise false.</returns>
        Task<(bool result, byte[] bytes)> TryGetPictureAsync(int categoryId);

        /// <summary>
        /// Update a product category picture.
        /// </summary>
        /// <param name="categoryId">A product category identifier.</param>
        /// <param name="stream">A <see cref="Stream"/>.</param>
        /// <returns>True if a product category is exist; otherwise false.</returns>
        Task<bool> UpdatePictureAsync(int categoryId, Stream stream);

        /// <summary>
        /// Destroy a product category picture.
        /// </summary>
        /// <param name="categoryId">A product category identifier.</param>
        /// <returns>True if a product category is exist; otherwise false.</returns>
        Task<bool> DestroyPictureAsync(int categoryId);
    }
}
