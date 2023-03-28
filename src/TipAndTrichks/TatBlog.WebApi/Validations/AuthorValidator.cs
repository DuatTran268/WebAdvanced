using FluentValidation;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations
{
	public class AuthorValidator : AbstractValidator<AuthorEditModel>
	{
		public AuthorValidator() 
		{
			RuleFor(a => a.FullName)
				.NotEmpty().WithMessage("Name author not empty ")
				.MaximumLength(100).WithMessage("Name author up to 100 characters");

			RuleFor(a => a.UrlSlug)
				.NotEmpty().WithMessage("UrlSlug not empty")
				.MaximumLength(100).WithMessage("UrlSlug up to 100 characters");

			RuleFor(a => a.JoinedDate)
				.GreaterThan(DateTime.MinValue)  // graterthan: toán tử lớn hơn
				.WithMessage("Joined Date not not valid");

			RuleFor(a => a.Email)
				.NotEmpty().WithMessage("Email not empty")
				.MaximumLength(100).WithMessage("Email up to 100 characters");

			RuleFor(a => a.Notes)
				.MaximumLength(500).WithMessage("Notes up to 500 characters");


				
		
		}
	}
}
