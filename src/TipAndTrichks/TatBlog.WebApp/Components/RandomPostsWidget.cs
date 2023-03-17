using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
	public class RandomPostsWidget : ViewComponent
	{
		private readonly IBlogRepository _blogRepository;

		public RandomPostsWidget(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			// lay danh sach bai viet duoc xem nhieu nhat
			var posts = await _blogRepository.GetRandomNPost(5);
			return View(posts);
		}
	}
}
