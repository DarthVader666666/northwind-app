using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Northwind.Services.Blogging;

namespace Northwind.Services.Interfaces
{
    public interface IBloggingService
    {
        /// <summary>
        /// Shows a list of blogArticles using specified offset and limit for pagination.
        /// </summary>
        /// <returns>A <see cref="IAsyncEnumerable{T}"/> of <see cref="BlogArticle"/>.</returns>
        IAsyncEnumerable<BlogArticle> GetBlogArticlesAsync();

        /// <summary>
        /// Try to show a blogArticle with specified identifier.
        /// </summary>
        /// <param name="blogArticleId">A blogArticle identifier.</param>
        /// <returns>Returns true if a blogArticle is returned; otherwise false.</returns>
        Task<(bool result, BlogArticle blog)> TryGetBlogArticleAsync(int blogArticleId);

        /// <summary>
        /// Creates a new blogArticle.
        /// </summary>
        /// <param name="blogArticle">A <see cref="BlogArticle"/> to create.</param>
        /// <returns>An identifier of a created blogArticle.</returns>
        Task<int> CreateBlogArticleAsync(BlogArticle blogArticle);

        /// <summary>
        /// Destroys an existed blogArticle.
        /// </summary>
        /// <param name="blogArticleId">A blogArticle identifier.</param>
        /// <returns>True if a blogArticle is destroyed; otherwise false.</returns>
        Task<bool> DestroyBlogArticleAsync(int blogArticleId);

        /// <summary>
        /// Looks up for blogArticle with specified names.
        /// </summary>
        /// <param name="names">A list of blogArticle names.</param>
        /// <returns>A list of blogArticles with specified names.</returns>
        //IAsyncEnumerable<BlogArticle> LookupBlogArticlesByNameAsync(List<string> names);

        /// <summary>
        /// Updates a blogArticle.
        /// </summary>
        /// <param name="blogArticleId">A blogArticle identifier.</param>
        /// <param name="blogArticle">A <see cref="BlogArticle"/>.</param>
        /// <returns>True if a blogArticle is updated; otherwise false.</returns>
        Task<bool> UpdateBlogArticleAsync(int blogArticleId, BlogArticle blogArticle);

        /// <summary>
        /// GetProductsForBlogArticleAsync.
        /// </summary>
        /// <param name="blogArticleId"></param>
        /// <returns></returns>
        IAsyncEnumerable<BlogArticleProduct> GetBlogArticleProductsAsync(int blogArticleId);

        /// <summary>
        /// CreateBlogArticleProduct.
        /// </summary>
        /// <param name="blogArticle"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<bool> CreateBlogArticleProductAsync(int blogArticleId, int productId);

        /// <summary>
        /// DestroyBlogArticleProductAsync.
        /// </summary>
        /// <param name="blogArticle"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<bool> DestroyBlogArticleProductAsync(int blogArticle, int productId);

        /// <summary>
        /// CreateCommentAsync.
        /// </summary>
        /// <param name="blogArticle"></param>
        /// <returns></returns>
        Task<int> CreateCommentAsync(int blogArticle, int customerId, string text);

        /// <summary>
        /// ReadAllComments.
        /// </summary>
        /// <param name="blogArticleId"></param>
        /// <returns></returns>
        IAsyncEnumerable<BlogComment> ReadAllComments(int blogArticleId);

        /// <summary>
        /// UpdateCommentAsync.
        /// </summary>
        /// <param name="blogArticleId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<bool> UpdateCommentAsync(int blogArticleId, int customerId, string text);

        /// <summary>
        /// DestroyCommentAsync.
        /// </summary>
        /// <param name="blogArticleId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<bool> DestroyCommentAsync(int blogArticleId, int customerId);
    }
}
