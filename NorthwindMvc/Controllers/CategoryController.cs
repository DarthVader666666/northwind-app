using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Northwind.Services.Products;
using NorthwindMvc.Extensions;
using NorthwindMvc.Models;
using NorthwindMvc.Models.CategoryModels;

namespace NorthwindMvc.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IMapper mapper;
        private const int PageSize = 7;
        private const string BasePath = "/api/categories";
        private readonly HttpClient httpClient;
        private readonly byte[] OleHeader = new byte[] { 21, 28, 47, 0, 2, 0, 0, 0, 13, 0, 14, 0, 20, 
            0, 33, 0, 255, 255, 255, 255, 66, 105, 116, 109, 97, 112, 32, 73, 109, 97, 103, 101, 0, 80, 97,
            105, 110, 116, 46, 80, 105, 99, 116, 117, 114, 101, 0, 1, 5, 0, 0, 2, 0, 0, 0, 7, 0, 0, 0, 80,
            66, 114, 117, 115, 104, 0, 0, 0, 0, 0, 0, 0, 0, 0, 160, 41, 0, 0 };

        public CategoryController(HttpClient client, IMapper mapper)
        {
            this.httpClient = client ?? throw new ArgumentNullException("client");
            this.mapper = mapper ?? throw new ArgumentNullException();
        }

        public async Task<IActionResult> IndexAsync(int page = 1)
        {
            var count = int.Parse(await this.httpClient.GetStringAsync(BasePath + "/total"));

            var json = await this.httpClient.GetStringAsync(
                BasePath + $"?offset={(page - 1) * PageSize}&limit={PageSize}");

            var categories = JsonConvert.DeserializeObject<List<ProductCategory>>(json);

            var viewModel = new CategoryListModel()
            {
                Categories = categories.
                Select(x =>
                {
                    var c = this.mapper.Map<ProductCategory, CategoryModel>(x);
                    c.Picture = c.Picture.HasHeader(this.OleHeader) ? c.Picture[this.OleHeader.Length..] : c.Picture;
                    return c;
                }),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = count
                }
            };

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(CategoryModel categotyModel)
        {
            var categoty = this.mapper.Map<ProductCategory>(categotyModel);

            using var reader = new BinaryReader(categotyModel.PictureForm.OpenReadStream());
            categoty.Picture = reader.ReadBytes((int)categotyModel.PictureForm.Length);

            var json = JsonConvert.SerializeObject(categoty);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            await this.httpClient.PostAsync(BasePath, data);

            return this.RedirectToAction("Index");
        }
    }
}
