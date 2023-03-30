using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Models.Category;
using TatBlog.WebApi.Models.Post;

namespace TatBlog.WebApi.Endpoints;

public static class CategoryEndpoints
{
	public static WebApplication MapCategoryEndpoint(this WebApplication app)
	{
		// getcategory
		var routeGroupBuilder = app.MapGroup("/api/categorys");
		routeGroupBuilder.MapGet("/", GetCategoies)
			.WithName("GetCategoies")
			.Produces<PaginationResult<CategoryItem>>();


		// get categories details
		routeGroupBuilder.MapGet("/{id:int}", GetCategoriesDetailsById)
			.WithName("GetCategoriesDetailsById")
			.Produces<CategoryItem>()
			.Produces(404);

		// get post by author slug
		routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}/posts", GetPostsByCategoriesSlug)
			.WithName("GetPostsByCategoriesSlug")
			.Produces<PaginationResult<CategoryDto>>();

		// add category
		routeGroupBuilder.MapPost("/", AddCategory)
			.WithName("AddCategories")
			.AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
			.Produces(201)
			.Produces(400)
			.Produces(409);


		// update category
		routeGroupBuilder.MapPut("/{id:int}", UpdateCategoryById)
			.WithName("UpdateCategoryById")
			.AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
			.Produces(201)
			.Produces(400)
			.Produces(409);

		// delete category 
		routeGroupBuilder.MapDelete("/{id:int}", DeleteCategoryById)
			.WithName("DeleteCategoryById")
			.Produces(204)
			.Produces(404);
		return app;
	}


	private static async Task<IResult> GetCategoies(
		[AsParameters] CategoryFilterModel model, IBlogRepository blogRepository)
	{
		var categoryList = await blogRepository.GetPagedCategoryAsync(model, model.Name);

		var paginationResult = new PaginationResult<CategoryItem>(categoryList);

		return Results.Ok(paginationResult);

	}

	// get category detail by Id
	private static async Task<IResult> GetCategoriesDetailsById(
		int id, IBlogRepository blogRepository, IMapper mapper
		)
	{
		var category = await blogRepository.GetCachedCategoryByIdAsync(id);
		return category == null
			? Results.NotFound($"Không tìm thấy category có mã số {id}")
			:Results.Ok(mapper.Map<CategoryItem>(category));
	}

	// get post by category id
	private static async Task<IResult> GetPostsByCategoriesSlug(
		[FromRoute] string slug,
		[AsParameters] PagingModel pagingModel,
		IBlogRepository blogRepository
		)
	{
		var postQuery = new PostQuery()
		{
			AuthorSlug = slug,
			PublishedOnly = true
		};
		var postList = await blogRepository.GetPagePostsAsync(
				postQuery, pagingModel,
				posts => posts.ProjectToType<CategoryDto>());

		var paginationResult = new PaginationResult<CategoryDto>(postList);

		return Results.Ok(paginationResult);

	}

	// add new post

	private static async Task<IResult> AddCategory(
		CategoryEditModel model,
		IBlogRepository blogRepository,
		IMapper mapper)
	{
		if (await blogRepository.IsCategoriesSlugExistedAsync(0, model.UrlSlug))
		{
			return Results.Conflict($"Slug '{model.UrlSlug}' đã được sử dụng");
		}
		var categories = mapper.Map<Category>(model);
		await blogRepository.AddOrUpdateCateAsync(categories);

		return Results.CreatedAtRoute("AddCategories",
		new { categories.Id },
		mapper.Map<CategoryItem>(categories));
	}

	// update category by id
	private static async Task<IResult> UpdateCategoryById(
		int id, CategoryEditModel model, IBlogRepository blogRepository, IMapper mapper)
	{
		if (await blogRepository.IsCategoriesSlugExistedAsync(id, model.UrlSlug))
		{
			return Results.Conflict($"Slug '{model.UrlSlug}' đã được sử dụng");
		}
		var category = mapper.Map<Category>(model);
		category.Id = id;

		return await blogRepository.AddOrUpdateCateAsync(category)
			? Results.NoContent()
			: Results.NotFound();

	}


	// delete category by id
	private static async Task<IResult> DeleteCategoryById(
		int id, IBlogRepository blogRepository)
	{
		return await blogRepository.DeleteCategoryById(id)
			? Results.NoContent()
			: Results.NotFound($"Không tìm thấy category với id = {id}");
	}


}
