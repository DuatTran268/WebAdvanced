using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations
{
	public class TagValidator : AbstractValidator<TagEditModel>
	{
		private readonly IBlogRepository _blogRepository;
		public TagValidator(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;
			RuleFor(t => t.Name).NotEmpty().MaximumLength(500);
			RuleFor(t => t.UrlSlug).NotEmpty().MaximumLength(500);
			RuleFor(t => t.Description).NotEmpty().MaximumLength(1000);
		}
	}
}
