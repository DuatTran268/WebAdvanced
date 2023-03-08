﻿using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
    public class CategoriesWidget : ViewComponent
    {
        private readonly IBlogRepository _blogRepository;

        public CategoriesWidget(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // lay danh sach chu de
            var categories = await _blogRepository.GetCategoryAsync();
            return View(categories);
        }
    }
}
