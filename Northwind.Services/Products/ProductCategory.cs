namespace Northwind.Services.Products
{
    /// <summary>
    /// Represents a TO for Northwind product categories.
    /// </summary>
    public sealed class ProductCategory
    {
        /// <summary>
        /// Gets or sets a product category identifier.
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets a product category name.
        /// </summary>
        public string? CategoryName { get; set; }

        /// <summary>
        /// Gets or sets a product category description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets a product category picture.
        /// </summary>
        public byte[]? Picture { get; set; }
    }
}
