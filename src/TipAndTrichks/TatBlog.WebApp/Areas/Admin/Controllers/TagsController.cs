﻿using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class TagsController :Controller
    {
		private readonly IBlogRepository _blogRepository;
		private readonly IMapper _mapper;
		
		public TagsController (
			IBlogRepository blogRepository, 
			IMapper mapper)
		{
			_blogRepository = blogRepository;
			_mapper = mapper;
		}
		
		
		//public IActionResult Index()
		//{
		//	return View();
		//}


		public async Task<IActionResult> Index(
			TagFilterModel model,
			[FromQuery(Name ="p")] int pageNumber = 1,
			[FromQuery(Name ="ps")] int pageSize = 8
			)
		{
			var tagQuery = _mapper.Map<TagQuery>(model);

			ViewBag.TagList = await _blogRepository.GetPageTagAsync(tagQuery, pageNumber, pageSize);
			return View(model);
		}
	}
}
