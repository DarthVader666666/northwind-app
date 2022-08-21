using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        private static byte[] OleHeader = new byte[] { 21, 28, 47, 0, 2, 0, 0, 0, 13, 0, 14, 0, 20, 
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

            if (count == 0)
            {
                return this.View(new CategoryListModel());
            }

            var json = await this.httpClient.GetStringAsync(
                BasePath + $"?offset={(page - 1) * PageSize}&limit={PageSize}");

            var categories = JsonConvert.DeserializeObject<List<ProductCategory>>(json);

            var viewModel = new CategoryListModel()
            {
                Categories = categories.
                Select(x =>
                {
                    var c = this.mapper.Map<ProductCategory, CategoryModel>(x);
                    c.Picture = c.Picture.HasHeader(OleHeader) ? c.Picture[OleHeader.Length..] : c.Picture;
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

        [HttpGet]
        public async Task<IActionResult> DeleteAsync(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var json = await (await this.httpClient.GetAsync(BasePath + "/" + id)).Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<CategoryModel>(json);

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
            var obj = JsonConvert.DeserializeObject<CategoryModel>(json);

            if (obj == null)
            {
                return this.NotFound();
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
            var json = await this.httpClient.GetStringAsync(BasePath + "/" + id);
            var category = JsonConvert.DeserializeObject<ProductCategory>(json);
            var categoryModel = this.mapper.Map<ProductCategory, CategoryModel>(category);
            categoryModel.Picture = ConvertNorthwindPhoto(category.Picture);

            categoryModel.PictureHexString = BitConverter.ToString(categoryModel.Picture)!;

            return this.View(categoryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAsync(CategoryModel categoryModel)
        {
            var category = this.mapper.Map<CategoryModel, ProductCategory>(categoryModel);

            if (categoryModel.PictureForm is not null)
            {
                using var reader = new BinaryReader(categoryModel.PictureForm.OpenReadStream());
                category.Picture = reader.ReadBytes((int)categoryModel.PictureForm.Length);
            }
            else
            {
                var hex = categoryModel.PictureHexString is not null ?
                    new string(categoryModel.PictureHexString.Where(x => x != '-').ToArray()) : null;

                category.Picture = hex is not null ? Convert.FromHexString(hex) : null;
            }

            var json = JsonConvert.SerializeObject(category);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            await this.httpClient.PutAsync(BasePath + "/" + categoryModel.CategoryId, data);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> FindByNameAsync(CategoryListModel model)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, this.httpClient.BaseAddress +
                "api/categories/by_name");

            var json = JsonConvert.SerializeObject(new string[] { model.PagingInfo.Name });
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await this.httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return this.View(new List<CategoryModel>());
            }

            json = await response.Content.ReadAsStringAsync();

            var categories = JsonConvert.DeserializeObject<List<ProductCategory>>(json);

            var viewModel = categories is null ?
                new List<CategoryModel>() :
                categories.Select(x =>
                {
                    x.Picture = x.Picture.HasHeader(OleHeader) ? x.Picture[OleHeader.Length..] : x.Picture;
                    return this.mapper.Map<ProductCategory, CategoryModel>(x);
                });

            return this.View(viewModel);
        }

        private static byte[] ConvertNorthwindPhoto(byte[] source) =>
            source.HasHeader(OleHeader) ? source[OleHeader.Length..] : source;
    }
}
