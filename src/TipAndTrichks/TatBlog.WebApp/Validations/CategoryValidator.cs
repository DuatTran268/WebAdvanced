using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations
{
	public class CategoryValidator: AbstractValidator<CategoryEditModel>
	{
		private readonly IBlogRepository _blogRepository;

		public CategoryValidator(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;

			RuleFor(x => x.Name).NotEmpty().MaximumLength(500);

			RuleFor(x => x.UrlSlug).NotEmpty().MaximumLength(500);

			RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
		}

	
	}
}
