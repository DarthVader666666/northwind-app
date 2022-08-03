using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Northwind.Services.Products;
using NorthwindMvc.Models;
using NorthwindMvc.Models.ProductModels;

namespace NorthwindMvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMapper mapper;
        private const int PageSize = 12;
        private const string BasePath = "/api/products";
        private readonly HttpClient httpClient;

        public ProductController(HttpClient client, IMapper mapper)
        {
            this.httpClient = client ?? throw new ArgumentNullException();
            this.mapper = mapper ?? throw new ArgumentNullException();
        }

        public async Task<ActionResult> IndexAsync(int page = 1)
        {
            var count = int.Parse(await this.httpClient.GetStringAsync(BasePath + "/total"));

            var json = await this.httpClient.GetStringAsync(
                BasePath + $"?offset={(page - 1) * PageSize}&limit={PageSize}");

            var products = JsonConvert.DeserializeObject<List<Product>>(json);

            var viewModel = new ProductListModel()
            {
                Products = products.
                Select(x => this.mapper.Map<Product, ProductModel>(x)),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = count
                }
            };

            return this.View(viewModel);
        }

        public IActionResult CreateAsync()
        { 
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(ProductModel productModel)
        {
            var product = this.mapper.Map<Product>(productModel);
            var json = JsonConvert.SerializeObject(product);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            await this.httpClient.PostAsync(BasePath, data);

            return this.RedirectToAction("Index");
        }
    }
}
