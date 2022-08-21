using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Northwind.Services.Blogging;
using Northwind.Services.Employees;
using Northwind.Services.Products;
using NorthwindMvc.Models;
using NorthwindMvc.Models.BlogArticleModels;
using NorthwindMvc.Models.EmployeeModels;
using NorthwindMvc.Models.ProductModels;

namespace NorthwindMvc.Controllers
{
    public class BlogArticleController : Controller
    {
        private readonly IMapper mapper;
        private const int PageSize = 8;
        private const string BasePath = "/api/articles";
        private readonly HttpClient httpClient;

        public BlogArticleController(HttpClient client, IMapper mapper)
        {
            this.httpClient = client ?? throw new ArgumentNullException();
            this.mapper = mapper ?? throw new ArgumentNullException();
        }

        public async Task<IActionResult> IndexAsync(int page = 1)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, BasePath);
            var response = await this.httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return this.View(new BlogArticleListModel());
            }

            var json = await response.Content.ReadAsStringAsync();

            var allArticles = JsonConvert.DeserializeObject<List<BlogArticleModel>>(json);
            var count = allArticles.Count;

            var articles = allArticles.OrderBy(x => x.ArticleId).Skip((page - 1) * PageSize).Take(PageSize);

            var viewModel = new BlogArticleListModel()
            {
                Articles = articles,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = count
                }
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> CreateAsync()
        {
            var count = int.Parse(await this.httpClient.GetStringAsync("api/employees" + "/total"));

            var json = await this.httpClient.GetStringAsync(
                "api/employees" + $"?offset={0}&limit={count}");

            var employees = JsonConvert.DeserializeObject<List<Employee>>(json);

            var viewModel = new BlogArticleCreateModel
            {
                Authors = employees.Select(x => this.mapper.Map<Employee, EmployeeModel>(x)),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePostAsync(BlogArticleCreateModel articleCreateModel)
        {


            var article = this.mapper.Map<BlogArticleModel, BlogArticle>(articleCreateModel.BlogArticle);
            var json = JsonConvert.SerializeObject(article);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            await this.httpClient.PostAsync(BasePath, data);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAsync(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var json = await (await this.httpClient.GetAsync(BasePath + "/" + id)).Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<BlogArticleModel>(json);

            if (obj == null)
            {
                return this.NotFound();
            }

            return this.View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePostAsync(int? id)
        {
            var json = await (await this.httpClient.GetAsync(BasePath + "/" + id)).Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<BlogArticleModel>(json);

            if (obj == null)
            {
                return this.NotFound();
            }

            json = await (await this.httpClient.GetAsync(BasePath + $"/{id}/products")).Content.ReadAsStringAsync();
            var linkedProductsIds = JsonConvert.DeserializeObject<List<Product>>(json).Select(x => x.ProductId);

            foreach (var item in linkedProductsIds)
            {
                await this.httpClient.DeleteAsync(BasePath + $"/{id}/products/{item}");
            }

            var response = await this.httpClient.DeleteAsync(BasePath + "/" + id);

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return this.StatusCode(500, await response.Content.ReadAsStringAsync());
            }

            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateAsync(int id)
        {
            var count = int.Parse(await this.httpClient.GetStringAsync("api/employees" + "/total"));

            var json = await this.httpClient.GetStringAsync(
                "api/employees" + $"?offset={0}&limit={count}");

            var employess = JsonConvert.DeserializeObject<List<Employee>>(json);

            json = await this.httpClient.GetStringAsync(BasePath + "/" + id);

            var authorId = int.Parse(JsonConvert.DeserializeObject<Dictionary<string, string>>(json)["authorId"]);
            var article = JsonConvert.DeserializeObject<BlogArticle>(json);

            var viewModel = new BlogArticleCreateModel
            {
                BlogArticle = this.mapper.Map<BlogArticleModel>(article),
                Authors = employess.Select(x => this.mapper.Map<Employee, EmployeeModel>(x))
            };

            viewModel.BlogArticle.EmployeeId = authorId;

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAsync(BlogArticleCreateModel articleModel)
        {
            var article = this.mapper.Map<BlogArticle>(articleModel.BlogArticle);
            article.PublishDate = DateTime.Now;

            var json = JsonConvert.SerializeObject(article);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            await this.httpClient.PutAsync(BasePath + "/" + articleModel.BlogArticle.ArticleId, data);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> FindByTitleAsync(BlogArticleListModel model)
        {
            if (model.PagingInfo.Name is null)
            {
                return this.View(new List<BlogArticleModel>());
            }

            var json = await this.httpClient.GetStringAsync(BasePath);
            var articleIds = JsonConvert.DeserializeObject<IEnumerable<Dictionary<string, string>>>(json).
                Select(x => int.Parse(x["articleId"]));

            var articles = GetArticlesAsync();

            var viewModel = articles is null || !(await articles.AnyAsync()) ? new List<BlogArticleModel>() :
                await articles.Where(x => x.Title.Contains(model.PagingInfo.Name, StringComparison.OrdinalIgnoreCase)).ToListAsync();

            return this.View(viewModel);

            async IAsyncEnumerable<BlogArticleModel> GetArticlesAsync()
            {
                foreach (var id in articleIds)
                {
                    json = await this.httpClient.GetStringAsync(BasePath + $"/{id}");
                    var article = JsonConvert.DeserializeObject<BlogArticleModel>(json);
                    var body = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    article.PostedDate = DateTime.Parse(body["posted"]);

                    yield return article;
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddProductLink(int id)
        {
            int count = int.Parse(await this.httpClient.GetStringAsync("/api/products/total"));
            var json = await this.httpClient.GetStringAsync($"/api/products?offset={0}&limit={count}");
            var products = JsonConvert.DeserializeObject<List<Product>>(json);

            var request = new HttpRequestMessage(HttpMethod.Get, BasePath + $"/{id}/products");
            var response = await this.httpClient.SendAsync(request);
            var linkedProducts = new List<Product>();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                json = await response.Content.ReadAsStringAsync();
                linkedProducts = JsonConvert.DeserializeObject<List<Product>>(json);
            }

            var viewModel = new ProductLinksModel
            {
                ArticleId = id,
                Products = products.Select(x => this.mapper.Map<ProductModel>(x)).ToList(),
            };

            var links = from p in viewModel.Products
                        from pl in linkedProducts
                        where p.ProductId == pl.ProductId
                        select p;

            foreach (var item in links)
            {
                item.IsSelected = true;
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductLinkPost(ProductLinksModel linksModel)
        {
            foreach (var item in linksModel.Products)
            {
                if (item.IsSelected)
                {
                    await this.httpClient.PostAsync(
                    BasePath + $"/{linksModel.ArticleId}/products/{item.ProductId}", null);
                }
                else
                {
                    await this.httpClient.DeleteAsync(
                    BasePath + $"/{linksModel.ArticleId}/products/{item.ProductId}");
                }
            }

            return this.RedirectToAction("Index");
        }
    }
}
