﻿using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using Northwind.Services.EntityFrameworkCore.Context;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.Interfaces;

namespace Northwind.Services.EntityFrameworkCore
{
    public class ProductCategoryPicturesManagementService : IProductCategoryPicturesManagementService
    {
        private readonly NorthwindContext context;

        public ProductCategoryPicturesManagementService(NorthwindContext context)
        {
            this.context = context;
        }

        public async Task<bool> DestroyCategoryPictureAsync(int categoryId)
        {
            var category = await this.context.Categories.FindAsync(categoryId);

            if (category is null)
            {
                return false;
            }

            category.Picture = null;
            await this.context.SaveChangesAsync();
            return true;
        }

        public async Task<(bool, byte[])> TryGetCategoryPictureAsync(int categoryId)
        {
            var category = await this.context.Categories.FindAsync(categoryId);

            return (category.Picture is not null, category.Picture);
        }

        public async Task<bool> UpdateCategoryPictureAsync(int categoryId, Stream stream)
        {
            var category = await this.context.Categories.FindAsync(categoryId);

            if (category is null)
            {
                return false;
            }

            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            category.Picture = memoryStream.ToArray();

            this.context.Entry(category).State = EntityState.Modified;
            await this.context.SaveChangesAsync();

            return category.Picture.Length > 0;
        }
    }
}
