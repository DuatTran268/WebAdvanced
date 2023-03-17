using Microsoft.AspNetCore.Mvc;
using SlugGenerator;
using System.Drawing.Printing;
using TatBlog.Core;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
	public class BestAuthorsWidget : ViewComponent
	{
		public readonly IBlogRepository _blogRepository;

		public BestAuthorsWidget(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			IPagingParams pagingParams = new PagingParams()
			{
				PageNumber = 1,
				PageSize = 4
			};
			var author = await _blogRepository.GetNAuthorTopPosts(4, pagingParams);

			return View(author);
		}

	}
}