using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
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
		var routeGroupBuilder = app.MapGroup("/api/categories");
		routeGroupBuilder.MapGet("/listcate", GetCategoies)
			.WithName("GetCategoies")
			.Produces<ApiResponse<PaginationResult<CategoryItem>>>();

		routeGroupBuilder.MapGet("/", GetCategories2)
			.WithName("GetCategories2")
			.Produces<ApiResponse<IList<CategoryItem>>>();


		// get categories details
		routeGroupBuilder.MapGet("/{id:int}", GetCategoriesDetailsById)
			.WithName("GetCategoriesDetailsById")
			.Produces<ApiResponse<CategoryItem>>();

		// get post by author slug
		routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}", GetPostsByCategoriesSlug)
			.WithName("GetPostsByCategoriesSlug")
			.Produces<ApiResponse<PaginationResult<PostDto>>>();

		// add category
		routeGroupBuilder.MapPost("/", AddCategory)
			.WithName("AddCategories")
			.AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
			.Produces(401)
			.Produces<ApiResponse<CategoryItem>>();


		// update category
		routeGroupBuilder.MapPut("/{id:int}", UpdateCategoryById)
			.WithName("UpdateCategoryById")
			.AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
			.Produces(401)
			.Produces<ApiResponse<string>>();

		// delete category 
		routeGroupBuilder.MapDelete("/{id:int}", DeleteCategoryById)
			.WithName("DeleteCategoryById")
			.Produces(401)
			.Produces<ApiResponse<string>>();
		
		return app;
	}

	// get category có phân trang
	private static async Task<IResult> GetCategoies(
		[AsParameters] CategoryFilterModel model, IBlogRepository blogRepository)
	{
		var categoryList = await blogRepository.GetPagedCategoryAsync(model, model.Name);

		var paginationResult = new PaginationResult<CategoryItem>(categoryList);

		return Results.Ok(ApiResponse.Success(paginationResult));

	}

	private static async Task<IResult> GetCategories2(
		IBlogRepository blogRepository)
	{
		var cateList = await blogRepository.GetCategoryAsync();
		return Results.Ok(ApiResponse.Success(cateList));
	}

	// get category detail by Id
	private static async Task<IResult> GetCategoriesDetailsById(
		int id, IBlogRepository blogRepository, IMapper mapper
		)
	{
		var category = await blogRepository.GetCachedCategoryByIdAsync(id);
		return category == null
			? Results.Ok(ApiResponse.Fail(
				HttpStatusCode.NotFound, $"Không tìm thấy category có mã số {id}"))
			: Results.Ok(ApiResponse.Success(mapper.Map<CategoryItem>(category)));
		
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
			CategorySlug = slug,
			PublishedOnly = true
		};
		var postList = await blogRepository.GetPagePostsAsync(
				postQuery, pagingModel,
				posts => posts.ProjectToType<PostDto>());

		var paginationResult = new PaginationResult<PostDto>(postList);
		return Results.Ok(ApiResponse.Success(paginationResult));
	}

	// add new post

	private static async Task<IResult> AddCategory(
		CategoryEditModel model,
		IBlogRepository blogRepository,
		IMapper mapper)
	{
		if (await blogRepository.IsCategoriesSlugExistedAsync(0, model.UrlSlug))
		{
			return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
				$"Slug '{model.UrlSlug}' đã được sử dụng"));
		}
		var categories = mapper.Map<Category>(model);
		await blogRepository.AddOrUpdateCateAsync(categories);

		return Results.Ok(ApiResponse.Success(mapper.Map<CategoryItem>(categories), HttpStatusCode.Created));

	}

	// update category by id
	private static async Task<IResult> UpdateCategoryById(
		int id, CategoryEditModel model, 
		IValidator<CategoryEditModel> validator,
		IBlogRepository blogRepository, IMapper mapper)
	{
		var validationResult = await validator.ValidateAsync(model);
		if (!validationResult.IsValid)
		{
			return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, validationResult));
		}

		if (await blogRepository.IsCategoriesSlugExistedAsync(id, model.UrlSlug))
		{
			return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));

		}
		var category = mapper.Map<Category>(model);
		category.Id = id;

		return await blogRepository.AddOrUpdateCateAsync(category)
					? Results.Ok(ApiResponse.Success("Category is updated", HttpStatusCode.NoContent))
					: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find category"));

	}


	// delete category by id
	private static async Task<IResult> DeleteCategoryById(
		int id, IBlogRepository blogRepository)
	{
		return await blogRepository.DeleteCategoryById(id)
			? Results.Ok(ApiResponse.Success("Category is deleted", HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find category"));
	}


}
