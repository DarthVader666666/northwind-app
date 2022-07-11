using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Services.Blogging
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
        Task<(bool result, BlogArticle blogArticle)> TryGetBlogArticleAsync(int blogArticleId);

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
        /// Try to show a blogArticleProduct with specified identifier.
        /// </summary>
        /// <param name="blogArticleId">A blogArticle identifier.</param>
        /// <returns>Returns true if a blogArticle is returned; otherwise false.</returns>
        Task<(bool result, BlogArticleProduct blogArticle)> TryGetBlogArticleProductAsync(int blogArticleId);

    }
}
