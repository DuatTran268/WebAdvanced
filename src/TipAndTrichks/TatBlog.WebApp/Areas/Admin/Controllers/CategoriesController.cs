using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Collections;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {
		private readonly IBlogRepository _blogRepository;
		private readonly IMapper _mapper;

		public CategoriesController(IBlogRepository blogRepository, IMapper mapper)
		{
			_blogRepository = blogRepository;
			_mapper = mapper;

		}
		//public IActionResult Index()
		//{
		//	return View();
		//}


		public async Task<IActionResult> Index(
			CategoryFilterModel model,
			[FromQuery(Name = "p")] int pageNumber = 1,
			[FromQuery(Name = "ps")] int pageSize = 5
			)
		{
			var cateQuery = _mapper.Map<CategoryQuery>(model);
			ViewBag.CateList = await _blogRepository.GetPageCategoryAsync(cateQuery, pageNumber, pageSize);

			return View(model);

		}

	}
}
