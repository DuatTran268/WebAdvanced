using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class DashboardController : Controller
	{
        private readonly IBlogRepository _blogRepository;

        public DashboardController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public async Task<IActionResult> Index()
        {
            ViewBag.PostCount = await _blogRepository.PostCountAsync();
            
            ViewBag.PostCountUnPubish = await _blogRepository.PostCountNonPublicAsync();
            
            ViewBag.CountCategory = await _blogRepository.CountCategoryAsync();
            return View();
        }
    }
}
