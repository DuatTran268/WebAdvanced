using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations
{
	public class PostValidator : AbstractValidator<PostEditModel>
	{
		private readonly IBlogRepository _blogRepository;

		public PostValidator(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;

			RuleFor(x => x.Title).NotEmpty().MaximumLength(500);

			RuleFor(x => x.ShortDescription).NotEmpty();
			
			RuleFor(x => x.Description).NotEmpty();

			RuleFor(x => x.Meta).NotEmpty().MaximumLength(1000);

			RuleFor(x => x.UrlSlug).NotEmpty().MaximumLength(1000);

			RuleFor(x => x.UrlSlug).MustAsync(async (postModel, slug, cancellationToken)
				=> !await blogRepository.IsPostSlugExistedAsync(postModel.Id, slug, cancellationToken))
				.WithMessage("Slug '{PropertyValue}' đã được sử dụng");

			RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Bạn phải chọn chủ đề cho bài viết");

			RuleFor(x => x.AuthorId).NotEmpty().WithMessage("Bạn phải chọn tác giả của bài viết");

			RuleFor(x => x.SelectedTags).Must(HasAtLeastOneTag).WithMessage("Bạn phải chọn chủ đề cho bài viết");


			When(x => x.Id <= 0, () =>
			{
				RuleFor(x => x.ImageFile)
				.Must(x => x is { Length: > 0 })
				.WithMessage("Bạn cần phải chọn hình ảnh cho bài viết");
			}).Otherwise(() =>
			{
				RuleFor(x => x.ImageFile)
				.MustAsync(SetImageIfNotExist)
				.WithMessage("Bạn phải chọn hình ảnh cho bài viết");
			});
		}

		// kiem tra nguoi dung nhap it nhat 1 the tag
		private bool HasAtLeastOneTag(PostEditModel postModel, string selectedTag)
		{
			return postModel.GetSlectedTags().Any();

		}

		private async Task<bool> SetImageIfNotExist(
			PostEditModel postModel, IFormFile imageFile, CancellationToken cancellationToken)
		{
			// lay thong tin tu csdl
			var post = await _blogRepository.GetPostByIdAsync(postModel.Id, false, cancellationToken);

			// bai viet co hinh anh ko bat buoc chon file
			if (!string.IsNullOrWhiteSpace(post?.ImageUrl))
				return true;

			// bai viet chua co hinh anh kiem tra nguoi dung chon file hay chua, neu chua bao loi
			return imageFile is { Length: > 0 };
		}

		
	}
}
