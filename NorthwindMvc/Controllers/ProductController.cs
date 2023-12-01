using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NorthwindMvc.Models;
using NorthwindMvc.Models.CategoryModels;
using NorthwindMvc.Models.ProductModels;

namespace NorthwindMvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMapper mapper;
        private const int PageSize = 9;
        private const string BasePath = "/api/products";
        private readonly HttpClient httpClient;

        public ProductController(HttpClient client, IMapper mapper)
        {
            this.httpClient = client ?? throw new ArgumentNullException();
            this.mapper = mapper ?? throw new ArgumentNullException();
        }

        public async Task<ActionResult> IndexAsync(string category = "", int page = 1)
        {
            int count = int.Parse(await this.httpClient.GetStringAsync(BasePath + "/total"));

            if (count == 0)
            {
                return this.View(new ProductListModel());
            }

            var json = await this.httpClient.GetStringAsync(
                BasePath + $"?offset={(page - 1) * PageSize}&limit={PageSize}");

            var products = JsonConvert.DeserializeObject<List<Product>>(json);

            if (category != string.Empty)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, this.httpClient.BaseAddress + "api/categories/by_name");
                json = JsonConvert.SerializeObject(new string[] { category });

                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await this.httpClient.SendAsync(request);
                json = await response.Content.ReadAsStringAsync();

                var categoryId = JsonConvert.DeserializeObject<List<ProductCategory>>(json).First().CategoryId;

                json = await this.httpClient.GetStringAsync(BasePath + $"?offset={0}&limit={count}");

                var allProducts = JsonConvert.DeserializeObject<List<Product>>(json).
                    Where(x => x.CategoryId == categoryId);

                count = allProducts.Count();

                products = allProducts.OrderBy(x => x.ProductId).
                    Skip((page - 1) * PageSize).Take(PageSize).ToList();
            }

            var viewModel = new ProductListModel
            {
                Products = products is null ? new List<ProductModel>() : products.Select(x => this.mapper.Map<Product, ProductModel>(x)),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = count,
                    Category = category
                }
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> CreateAsync()
        {
            var count = int.Parse(await this.httpClient.GetStringAsync("api/categories" + "/total"));

            var json = await this.httpClient.GetStringAsync(
                "api/categories" + $"?offset={0}&limit={count}");

            var categories = JsonConvert.DeserializeObject<List<ProductCategory>>(json);

            var viewModel = new ProductCreateModel
            {
                Categories = categories.Select(x => this.mapper.Map<ProductCategory, CategoryModel>(x))
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(ProductCreateModel productCreateModel)
        {
            var product = this.mapper.Map<Product>(productCreateModel.Product);
            var json = JsonConvert.SerializeObject(product);
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
            var obj = JsonConvert.DeserializeObject<ProductModel>(json);

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
            var obj = JsonConvert.DeserializeObject<ProductModel>(json);

            if (obj == null)
            {
                return this.NotFound();
            }

            await this.httpClient.DeleteAsync(BasePath + "/" + id);
            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateAsync(int id)
        {
            var count = int.Parse(await this.httpClient.GetStringAsync("api/categories" + "/total"));

            var json = await this.httpClient.GetStringAsync(
                "api/categories" + $"?offset={0}&limit={count}");

            var categories = JsonConvert.DeserializeObject<List<ProductCategory>>(json);

            json = await this.httpClient.GetStringAsync(BasePath + "/" + id);
            var product = JsonConvert.DeserializeObject<Product>(json);

            var viewModel = new ProductCreateModel
            {
                Product = this.mapper.Map<ProductModel>(product),
                Categories = categories.Select(x => this.mapper.Map<ProductCategory, CategoryModel>(x))
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAsync(ProductCreateModel productModel)
        {
            var product = this.mapper.Map<Product>(productModel.Product);
            var json = JsonConvert.SerializeObject(product);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            await this.httpClient.PutAsync(BasePath + "/" + productModel.Product.ProductId, data);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> FindByNameAsync(ProductListModel model)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, this.httpClient.BaseAddress +
                "api/products/by_name");

            var json = JsonConvert.SerializeObject(new string[] { model.PagingInfo.Name });
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await this.httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return this.View(new List<ProductModel>());
            }

            json = await response.Content.ReadAsStringAsync();

            var products = JsonConvert.DeserializeObject<List<Product>>(json);

            var viewModel = products is null ? new List<ProductModel>() : products.Select(x => this.mapper.Map<Product, ProductModel>(x));

            return this.View(viewModel);
        }
    }
}
