using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations
{
	public class AuthorValidator : AbstractValidator<AuthorEditModel>
	{
		private readonly IAuthorRepository _authorRepository;

		public AuthorValidator(IAuthorRepository authorRepository)
		{
			_authorRepository = authorRepository;

			RuleFor(x => x.FullNames).NotEmpty().MaximumLength(500);
			RuleFor(x => x.UrlSlug).NotEmpty().MaximumLength(500);
			RuleFor(x => x.Email).NotEmpty().MaximumLength(100);
			RuleFor(x => x.Note).NotEmpty().MaximumLength(1000);

		}

		private async Task<bool> SetImageIfNotExist(
			AuthorEditModel authorModel, IFormFile imageFile, CancellationToken cancellationToken)
		{
			// lay thong tin tu csdl
			var author = await _authorRepository.GetAuthorByIdAsync(authorModel.Id, false, cancellationToken);

			// bai viet co hinh anh ko bat buoc chon file
			if (!string.IsNullOrWhiteSpace(author?.ImageUrl))
				return true;

			// bai viet chua co hinh anh kiem tra nguoi dung chon file hay chua, neu chua bao loi
			return imageFile is { Length: > 0 };
		}
	}
}
