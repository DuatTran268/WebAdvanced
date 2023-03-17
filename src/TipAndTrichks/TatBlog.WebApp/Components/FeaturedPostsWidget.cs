using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
	public class FeaturedPostsWidget : ViewComponent
	{
		private readonly IBlogRepository _blogRepository;

		public FeaturedPostsWidget(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			// lay danh sach bai viet duoc xem nhieu nhat
			var posts = await _blogRepository.GetRandomNPost(3);
			return View(posts);
		}
	}
}
