using FluentValidation;
using TatBlog.WebApi.Models.Post;

namespace TatBlog.WebApi.Validations
{
	public class PostValidator : AbstractValidator<PostEditModel>
	{
		public PostValidator() 
		{ 
			RuleFor(p => p.Title)
				.NotEmpty()
				.WithMessage("Tên tiêu đề không được để trống")
				.MaximumLength(100)
				.WithMessage("Tên tiêu đề tối đa 100 ký tự");

			RuleFor(p => p.ShortDescription)
				.NotEmpty()
				.WithMessage("ShortDescription đề không được để trống")
				.MaximumLength(100)
				.WithMessage("ShortDesciption tối đa 100 ký tự");

			RuleFor(p => p.Description)
				.NotEmpty()
				.WithMessage("Description đề không được để trống")
				.MaximumLength(500)
				.WithMessage("Description tối đa 100 ký tự");

			RuleFor(p => p.Meta)
				.NotEmpty()
				.WithMessage("Meta đề không được để trống")
				.MaximumLength(500)
				.WithMessage("Meta tối đa 100 ký tự");

		}
	}
}
