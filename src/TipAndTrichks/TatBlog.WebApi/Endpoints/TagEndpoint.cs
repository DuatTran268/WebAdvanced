using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Models.Category;
using TatBlog.WebApi.Models.Post;
using TatBlog.WebApi.Models.Tag;

namespace TatBlog.WebApi.Endpoints
{
	public static class TagEndpoint
	{
		public static WebApplication MapTagEndpoints(this WebApplication app)
		{
			var routeGroupBuilder = app.MapGroup("/api/tags");

			routeGroupBuilder.MapGet("/", GetTag)
			.WithName("GetTag")
			.Produces<ApiResponse<IList<TagItem>>>();

			routeGroupBuilder.MapGet("/tagRequire", GetTags)
				.WithName("GetTags")
				.Produces<ApiResponse<PaginationResult<TagItem>>>();

			// get tag item
			routeGroupBuilder.MapGet("/{id:int}", GetTagDetails)
			.WithName("GetTagDetails")
			.Produces<ApiResponse<TagItem>>();


			// get post by tag slug
			routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}/tagSlug", GetPostsByTagsSlug)
				.WithName("GetPostsByTagsSlug")
				.Produces<ApiResponse<PaginationResult<PostDto>>>();

			// add tag
			routeGroupBuilder.MapPost("/", AddTags)
			.WithName("AddTags")
			.AddEndpointFilter<ValidatorFilter<TagEditModel>>()
			.Produces(401)
			.Produces<ApiResponse<TagItem>>();


			// update tag 
			routeGroupBuilder.MapPut("/{id:int}", UpdateTag)
			.WithName("UpdateTag")
			.AddEndpointFilter<ValidatorFilter<TagEditModel>>()
			.Produces(401)
			.Produces<ApiResponse<string>>();

			// delete tag

			routeGroupBuilder.MapDelete("/{id:int}", DeleteTags)
				.WithName("DeleteTags")
				.Produces(401)
				.Produces<ApiResponse<string>>();
			return app;
		}

		private static async Task<IResult> GetTag(
 			IBlogRepository blogRepository
			)
		{
			var tagList = await blogRepository.GetTagAsync();
			return Results.Ok(ApiResponse.Success(tagList));

		}

		private static async Task<IResult> GetTags(
			[AsParameters] TagFilterModel model,
			IBlogRepository blogRepository
			)
		{
			var tagList = await blogRepository.GetPagedTagsAsync(model, model.Name);

			var paginationResult = new PaginationResult<TagItem>(tagList);
			return Results.Ok(ApiResponse.Success(paginationResult));
		}


		// get tag details

		private static async Task<IResult> GetTagDetails(
			int id, IBlogRepository blogRepository, IMapper mapper
			)
		{
			var tags = await blogRepository.GetCachedTagByIdAsync(id);
			return tags == null
				? Results.Ok(ApiResponse.Fail(
					HttpStatusCode.NotFound, $"Không tìm thấy tag có mã số {id}"))
				: Results.Ok(ApiResponse.Success(mapper.Map<TagItem>(tags)));

		}

		private static async Task<IResult> GetPostsByTagsSlug(
		[FromRoute] string slug,
		[AsParameters] PagingModel pagingModel,
		IBlogRepository blogRepository)
		{
			var postQuery = new PostQuery()
			{
				TagSlug = slug,
			};

			var tagList = await blogRepository.GetPagePostsAsync(
				postQuery, pagingModel,
				posts => posts.ProjectToType<PostDto>());

			var paginationResult = new PaginationResult<PostDto>(tagList);

			return Results.Ok(ApiResponse.Success(paginationResult));
		}

		// add new tag
		private static async Task<IResult> AddTags(
			TagEditModel model, IBlogRepository blogRepository, IMapper mapper)
		{
			if (await blogRepository.IsTagSlugExistedAsync(0, model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
					$"Slug '{model.UrlSlug}' đã được sử dụng"));
			}

			var tag = mapper.Map<Tag>(model);
			await blogRepository.CreateOrUpdateTagAsync(tag);
			return Results.Ok(ApiResponse.Success(mapper.Map<TagItem>(tag), HttpStatusCode.Created));
		}


		// update tag 
		private static async Task<IResult> UpdateTag(
			int id, TagEditModel model,
			IValidator<TagEditModel> validator,
			IBlogRepository blogRepository,
			IMapper mapper)
		{
			var validationResult = await validator.ValidateAsync(model);
			if (!validationResult.IsValid)
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, validationResult));
			}

			if (await blogRepository.IsTagSlugExistedAsync(id, model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
			}
			var tag = mapper.Map<Tag>(model);
			tag.Id = id;

			return await blogRepository.AddOrUpdateTagAsync(tag)
				? Results.Ok(ApiResponse.Success("Author is updated", HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find author"));

		}

		private static async Task<IResult> DeleteTags(
			int id, IBlogRepository blogRepository)
		{
			return await blogRepository.DeleteTagById(id)
				? Results.Ok(ApiResponse.Success("Tag is deleted", HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find Tag"));
		}

	}
}
