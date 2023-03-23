using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class AuthorsController : Controller
	{
		private readonly IAuthorRepository _authorRepository;
		private readonly IMapper _mapper;

		public AuthorsController (IAuthorRepository authorRepository, IMapper mapper)
		{
			_authorRepository = authorRepository;
			_mapper = mapper;
		}

		//public IActionResult Index()
		//{
		//	return View();
		//}
		public async Task<IActionResult> Index(
			AuthorFilterModel model,
			[FromQuery(Name = "p")] int pageNumber = 1,
			[FromQuery(Name = "ps")] int pageSize = 5
			)
		{
			var authorQuery = _mapper.Map<AuthorQuery>(model);
			ViewBag.AuthorList = await _authorRepository.GetPageAuthorAsync(authorQuery, pageNumber, pageSize);

			return View(model);

		}



	}
}
