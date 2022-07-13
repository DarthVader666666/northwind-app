using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Northwind.Services;
using Northwind.Services.Entities;

namespace NorthwindApiApp.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IProductCategoriesManagementService categoryService;
        private readonly IProductCategoryPicturesManagementService pictureService;

        public ProductCategoriesController(IProductCategoriesManagementService categoryService,
            IProductCategoryPicturesManagementService pictureService)
        {
            this.categoryService = categoryService;
            this.pictureService = pictureService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Category>> ReadCategory(int id)
        {
            var task = await this.categoryService.TryGetCategoryAsync(id);

            return task.result ? this.Ok(task.category) : this.NotFound();
        }

        [HttpGet]
        public ActionResult<IAsyncEnumerable<Category>> ReadCategories(
            [FromQuery(Name = "offset")] int offset, [FromQuery(Name = "limit")] int limit)
        {
            var categories = this.categoryService.GetCategoriesAsync(offset, limit);
            return categories is not null ? this.Ok(categories) : this.BadRequest();
        }

        [HttpGet("by_name")]
        public ActionResult<IAsyncEnumerable<Category>> ReadCategories(List<string> names)
        {
            if (names is null || !names.Any())
            {
                return this.BadRequest();
            }

            var result = this.categoryService.LookupCategoriesByNameAsync(names);

            if (result is null || !result.AnyAsync().Result)
            {
                return this.NotFound("No categories with such names found.");
            }

            return this.Ok(result);
        }

        [HttpGet("{categoryId:int}/picture")]
        public async Task<IActionResult> ReadCategoryImage(int categoryId)
        {
            var (result, bytes) = await this.pictureService.TryGetPictureAsync(categoryId);

            if (result)
            {
                var memoryStream = new MemoryStream(bytes);
                return this.Ok(memoryStream);
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            if (category is null)
            {
                return this.BadRequest();
            }

            var id = await this.categoryService.CreateCategoryAsync(category);
            return category is not null ? this.Ok($"New category created Id = {id}") : this.BadRequest();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            var result = await this.categoryService.UpdateCategoriesAsync(id, category);
            return result ? this.NoContent() : this.NotFound($"Category with id = {id} not found.");
        }

        [HttpPut("{categoryId:int}/picture")]
        public async Task<IActionResult> UpdateCategoryImage(int categoryId)
        {
            var stream = this.HttpContext.Request.Form.Files[0].OpenReadStream();
            var result = await this.pictureService.UpdatePictureAsync(categoryId, stream);
            stream.Close();

            if (result)
            {
                return this.NoContent();
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpDelete("{categoryId:int}/picture")]
        public async Task<IActionResult> DeleteCategoryImage(int categoryId)
        {
            var result = await this.pictureService.DestroyPictureAsync(categoryId);

            if (result)
            {
                return this.NoContent();
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (id <= 0)
            {
                return this.NotFound($"Category Id = {id} NotFound.");
            }

            var result = await this.categoryService.DestroyCategoryAsync(id);

            return result ? this.NoContent() : this.NotFound($"Category Id = {id} NotFound.");
        }
    }
}
