using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class TagsController :Controller
    {
		private readonly IBlogRepository _blogRepository;
		private readonly IMapper _mapper;
		
		public TagsController (
			IBlogRepository blogRepository, 
			IMapper mapper)
		{
			_blogRepository = blogRepository;
			_mapper = mapper;
		}
		
		
		//public IActionResult Index()
		//{
		//	return View();
		//}


		public async Task<IActionResult> Index(
			TagFilterModel model,
			[FromQuery(Name ="p")] int pageNumber = 1,
			[FromQuery(Name ="ps")] int pageSize = 8
			)
		{
			var tagQuery = _mapper.Map<TagQuery>(model);

			ViewBag.TagList = await _blogRepository.GetPageTagAsync(tagQuery, pageNumber, pageSize);
			return View(model);
		}

		// click show details
		[HttpGet]
		public async Task<IActionResult> Edit(int id = 0)
		{
			var tag = id > 0
				? await _blogRepository.GetTagByIdAsync(id, true) : null;
			var model = tag == null
				? new TagEditModel()
				: _mapper.Map<TagEditModel>(tag);

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit([FromServices] IValidator<TagEditModel> tagValidator,
			TagEditModel model)
		{
			var validatorResult = await tagValidator.ValidateAsync(model);
			if (!validatorResult.IsValid)
			{
				validatorResult.AddToModelState(ModelState);
			}
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var tag = model.Id > 0
				? await _blogRepository.GetTagByIdAsync(model.Id)
				: null;
			if (tag == null)
			{
				tag = _mapper.Map<Tag>(model);
				tag.Id = 0;
			}
			else
			{
				_mapper.Map(model, tag);
			}

			await _blogRepository.CreateOrUpdateTagAsync(tag);

			return RedirectToAction(nameof(Index)); // trả về action
		}

	}
}
