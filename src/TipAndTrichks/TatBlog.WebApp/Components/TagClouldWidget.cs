using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components;

public class TagClouldWidget : ViewComponent
{
	private readonly IBlogRepository _blogRepository;

	public TagClouldWidget(IBlogRepository blogRepository)
	{
		_blogRepository = blogRepository;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		// lay danh sach chu de
		var tags = await _blogRepository.GetTagsAllAsync();
		return View(tags);
	}

}
