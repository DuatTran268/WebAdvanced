using FluentValidation;
using TatBlog.WebApi.Models.Author;
using TatBlog.WebApi.Models.Category;

namespace TatBlog.WebApi.Validations
{
	public class CategoryValidator : AbstractValidator<CategoryEditModel>
	{
		public CategoryValidator() 
		{
			RuleFor(c => c.Name)
				.NotEmpty()
				.WithMessage("Tên tiêu đề không được để trống")
				.MaximumLength(100)
				.WithMessage("Tên tiêu đề tối đa 100 ký tự");

			RuleFor(c => c.UrlSlug)
				.NotEmpty()
				.WithMessage("UrlSlug không được để trống")
				.MaximumLength(100)
				.WithMessage("UrlSlug tối đa 100 ký tự");

			RuleFor(c => c.Description)
				.NotEmpty()
				.WithMessage("Description không được để trống")
				.MaximumLength(200)
				.WithMessage("Description chứa tối đa 200 ký tự");

			RuleFor(c => c.ShowOnMenu);
		
		}
	}
}
