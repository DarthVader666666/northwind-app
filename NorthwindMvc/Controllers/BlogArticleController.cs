using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Northwind.Services.Blogging;
using NorthwindMvc.Models;
using NorthwindMvc.Models.BlogArticleModels;

namespace NorthwindMvc.Controllers
{
    public class BlogArticleController : Controller
    {
        private readonly IMapper mapper;
        private const int PageSize = 7;
        private const string BasePath = "/api/articles";
        private readonly HttpClient httpClient;

        public BlogArticleController(HttpClient client, IMapper mapper)
        {
            this.httpClient = client ?? throw new ArgumentNullException();
            this.mapper = mapper ?? throw new ArgumentNullException();
        }
        public async Task<IActionResult> IndexAsync(int page = 1)
        {
            var json = await this.httpClient.GetStringAsync(BasePath);

            var articles = JsonConvert.DeserializeObject<List<BlogArticleModel>>(json).
                OrderBy(x => x.ArticleId).
                Skip((page - 1) * PageSize).Take(PageSize);

            var count = articles.Count();

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

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(BlogArticleModel article)
        {
            return this.View();
        }
    }
}
