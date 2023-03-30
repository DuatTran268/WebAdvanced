using FluentValidation;
using TatBlog.WebApi.Models.Tag;

namespace TatBlog.WebApi.Validations
{
	public class TagValidator : AbstractValidator<TagEditModel>
	{
		public TagValidator() 
		{

			RuleFor(a => a.Name)
					.NotEmpty().WithMessage("Name tag not empty ")
					.MaximumLength(100).WithMessage("Name tag up to 100 characters");

			RuleFor(a => a.UrlSlug)
				.NotEmpty().WithMessage("UrlSlug not empty")
				.MaximumLength(100).WithMessage("UrlSlug up to 100 characters");

			RuleFor(a => a.Description)
				.NotEmpty().WithMessage("Description not empty")
				.MaximumLength(100).WithMessage("Description up to 100 characters");
		}
	}
}
