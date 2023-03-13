using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class PostsController : Controller
    {
		private readonly IBlogRepository _blogRepository;
		private readonly IMapper _mapper;


		public PostsController(IBlogRepository blogRepository, IMapper mapper)
		{
			_blogRepository = blogRepository;
			_mapper = mapper;
		}




		//public IActionResult Index()
		//{
		//	return View();
		//}

		public async Task<IActionResult> Index(PostFilterModel model)
		{
			//var postQuery = new PostQuery()
			//{
			//	Keyword = model.Keyword,
			//	CategoriesId = model.CategoryId,
			//	AuthorsId = model.AuthorId,
			//	PostedYear = model.Year,
			//	PostedMonths = model.Month
			//};

			// sử dụng mapster để tạo đối tượng PostQuery từ đối tượng PostFilterModel model
			var postQuery = _mapper.Map<PostQuery>(model);

			ViewBag.PostList = await _blogRepository.GetPagePostAsync(postQuery, 1, 10);
			await PopulatePostFilterModelAsync(model);
			return View(model);
		}


		private async Task PopulatePostFilterModelAsync(PostFilterModel model)
		{
			var author = await _blogRepository.GetAuthorAsync();
			var categories = await _blogRepository.GetCategoryAsync();

			model.AuthorList = author.Select(a => new SelectListItem()
			{
				Text = a.FullName,
				Value = a.AuthorId.ToString()
			});

			model.CategoryList = categories.Select(c => new SelectListItem()
			{
				Text = c.Name,
				Value = c.Id.ToString(),
			});
		}
	}
}
