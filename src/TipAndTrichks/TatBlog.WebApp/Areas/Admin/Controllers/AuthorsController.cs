using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Net.WebSockets;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class AuthorsController : Controller
	{
		private readonly IAuthorRepository _authorRepository;
		private readonly IMapper _mapper;
		private readonly IMediaManager _mediaManager;


		public AuthorsController (IAuthorRepository authorRepository, IMapper mapper, IMediaManager mediaManager)
		{
			_authorRepository = authorRepository;
			_mapper = mapper;
			_mediaManager = mediaManager;
		}

		//public IActionResult Index()
		//{
		//	return View();
		//}
		// corporate
		public async Task<IActionResult> Index(
			AuthorFilterModel model,
			[FromQuery(Name = "p")] int pageNumber = 1,
			[FromQuery(Name = "ps")] int pageSize = 5
			)
		{
			var authorQuery = _mapper.Map<AuthorQuery>(model);
			ViewBag.AuthorList = await _authorRepository.GetPageAuthorAsync(authorQuery, pageNumber, pageSize);

			return View(model);

		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id = 0)
		{
			var author = id > 0
				? await _authorRepository.GetAuthorByIdAsync(id, true) : null;

			// tao view model tu du lieu cua bai viet
			var model = author == null 
				? new AuthorEditModel()
				: _mapper.Map<AuthorEditModel>(author);
			
			return View(model);

		}

		[HttpPost]
		public async Task<IActionResult> Edit([FromServices] IValidator<AuthorEditModel> authorValidator
			, AuthorEditModel model)
		{
			var validationResult = await authorValidator.ValidateAsync(model);

			if (!validationResult.IsValid)
			{
				validationResult.AddToModelState(ModelState);
			}
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var author = model.Id > 0 
				? await _authorRepository.GetAuthorByIdAsync(model.Id) : null;

			if (author == null)
			{
				author = _mapper.Map<Author>(model);
				author.Id= 0;
				//author.JoinedDate = DateTime.Now;
			}
			else
			{
				_mapper.Map(model, author);
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
					await _mediaManager.DeleteFileAsync(author.ImageUrl);
					author.ImageUrl = newImagePath;
				}
			}

			await _authorRepository.CreateOrUpdateAuthorAsync(author);
			return RedirectToAction(nameof(Index));

		}


	}
}
