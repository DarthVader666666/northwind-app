using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NorthwindMvc.Models.CategoryModels;

namespace NorthwindMvc.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;
        private const string BasePath = "/api/categories";

        public NavigationMenuViewComponent(HttpClient client, IMapper mapper)
        { 
            this.httpClient = client ?? throw new ArgumentNullException();
            this.mapper = mapper ?? throw new ArgumentNullException();
        }

        public IViewComponentResult Invoke()
        {
            var count = int.Parse(this.httpClient.GetStringAsync(BasePath + "/total").Result);

            var json = this.httpClient.GetStringAsync(
                "/api/categories" + $"?offset={0}&limit={count}").Result;

            var categories = JsonConvert.DeserializeObject<List<ProductCategory>>(json);

            this.ViewBag.SelectedCategory = this.RouteData?.Values["category"];

            return this.View(categories.Select(x => this.mapper.Map<CategoryModel>(x).CategoryName).OrderBy(x => x));
        }
    }
}
