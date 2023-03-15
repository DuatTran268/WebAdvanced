using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class PostsController : Controller
    {
		private readonly IBlogRepository _blogRepository;
		private readonly IMapper _mapper;
		private readonly IMediaManager _mediaManager;
		private readonly ILogger<PostsController> _logger;

		public PostsController(IBlogRepository blogRepository, 
			IMediaManager mediaManager, 
			IMapper mapper
			, ILogger<PostsController> logger)
		{
			_blogRepository = blogRepository;
			_mediaManager = mediaManager;
			_mapper = mapper;
			_logger = logger;
		}

		// lọc tìm kiếm (filter and find)
		private async Task PopulatePostFilterModelAsync(PostFilterModel model)
		{
			var author = await _blogRepository.GetAuthorAsync();
			var categories = await _blogRepository.GetCategoryAsync();

			model.AuthorList = author.Select(a => new SelectListItem()
			{
				Text = a.FullName,
				Value = a.AuthorId.ToString()
			});

			model.CategoryList = categories.Select(c => new SelectListItem()
			{
				Text = c.Name,
				Value = c.Id.ToString(),
			});
		}

		// sửa (edit)
		private async Task PopulatePostEditModelAsync(PostEditModel model)
		{
			var authors = await _blogRepository.GetAuthorAsync();
			var categories = await _blogRepository.GetCategoryAsync();

			model.AuthorList = authors.Select(a => new SelectListItem()
			{
				Text = a.FullName,
				Value = a.AuthorId.ToString()
			});

			model.CategoryList = categories.Select(c => new SelectListItem()
			{
				Text = c.Name,
				Value = c.Id.ToString()
			});
		}

		public async Task<IActionResult> Index(PostFilterModel model)
		{
			//var postQuery = new PostQuery()
			//{
			//	Keyword = model.Keyword,
			//	CategoriesId = model.CategoryId,
			//	AuthorsId = model.AuthorId,
			//	PostedYear = model.Year,
			//	PostedMonths = model.Month
			//};
			_logger.LogInformation("Tạo điều kiện truy vấn");


			// sử dụng mapster để tạo đối tượng PostQuery từ đối tượng PostFilterModel model
			var postQuery = _mapper.Map<PostQuery>(model);

			ViewBag.PostList = await _blogRepository.GetPagePostAsync(postQuery, 1, 10);

			_logger.LogInformation("Chuẩn bị dữ liệu cho view model");

			await PopulatePostFilterModelAsync(model);
			return View(model);
		}


		[HttpGet]
		public async Task<IActionResult> Edit(int id = 0)
		{
			// id = 0: thêm bài viết mới
			// id > 0 đọc dữ liệu từ csdl
			var post = id > 0 ? await _blogRepository.GetPostByIdAsync(id, true) : null;

			// tạo viewmodel từ dữ liệu của bài viết
			var model = post == null 
				? new PostEditModel() 
				: _mapper.Map<PostEditModel>(post);

			// gan cac gia tri cho view Model
			await PopulatePostEditModelAsync(model);
			return View(model);

		}

		[HttpPost]
		public async Task<IActionResult> Edit([FromServices]
			IValidator<PostEditModel> postValidator,PostEditModel model)
		{
			var validationResult = await postValidator.ValidateAsync(model);

			if (!validationResult.IsValid)
			{
				validationResult.AddToModelState(ModelState);
			}

			if (!ModelState.IsValid)
			{
				await PopulatePostEditModelAsync(model);
				return View(model);
			}

			var post = model.Id > 0
				? await _blogRepository.GetPostByIdAsync(model.Id) : null;

			if (post == null)
			{
				post = _mapper.Map<Post>(model);
				post.Id = 0;

				post.PostedDate = DateTime.Now;
			}
			else
			{
				_mapper.Map(model, post);
				post.Category = null;
				post.ModifiedDate = DateTime.Now;
			}

			//upload hinh anh minh hoa cho bai viet
			if (model.ImageFile?.Length > 0)
			{
				// thuc hien luu tap tin vao thu muc upload
				var newImagePath = await _mediaManager.SaveFileAsync(
					model.ImageFile.OpenReadStream(),
					model.ImageFile.FileName,
					model.ImageFile.ContentType);

				// luu thanh cong xoa tap tin anh cu
				if (!string.IsNullOrWhiteSpace(newImagePath))
				{
					await _mediaManager.DeleteFileAsync(post.ImageUrl);
					post.ImageUrl = newImagePath;
				}
			}

			await _blogRepository.CreateOrUpdatePostAsync(post, model.GetSlectedTags());
			return RedirectToAction(nameof(Index));
		}



		[HttpPost]
		public async Task<IActionResult> VerifyPostSlug(int id, string urlSlug)
		{
			var slugExisted = await _blogRepository.IsPostSlugExistedAsync(id, urlSlug);

			return slugExisted ? Json($"Slug '{urlSlug}' đã được sử dụng ") : Json(true);
		}




	}
}
