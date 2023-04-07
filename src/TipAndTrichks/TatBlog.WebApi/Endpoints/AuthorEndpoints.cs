using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Models.Author;
using TatBlog.WebApi.Models.Post;

namespace TatBlog.WebApi.Endpoints
{
    public static class AuthorEndpoints
	{
		public static WebApplication MapAuthorEndpoints(this WebApplication app)
		{
			var routeGroupBuilder = app.MapGroup("/api/authors");


			routeGroupBuilder.MapGet("/", GetAuthors)
				.WithName("GetAuthors")
				.Produces<ApiResponse<PaginationResult<AuthorItem>>>();

			// get top author post
			routeGroupBuilder.MapGet("/best/{limit:int}", GetTopAuthor)
				.WithName("GetTopAuthor")
				.Produces<ApiResponse<AuthorItem>>();



			// get author details
			routeGroupBuilder.MapGet("/{id:int}", GetAuthorDetails)
				.WithName("GetAuthorsById")
				.Produces<ApiResponse<AuthorItem>>();

			//  get by author id
			routeGroupBuilder.MapGet("/{id:int}/listpost", GetPostsByAuthorId)
				.WithName("GetPostsByAuthorId")
				.Produces<ApiResponse<PaginationResult<PostDto>>>();

			// get post by author slug
			routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}/posts", GetPostsByAuthorSlug)
				.WithName("GetPostsByAuthorSlug")
				.Produces<ApiResponse<PaginationResult<PostDto>>>();

			// add author
			routeGroupBuilder.MapPost("/", AddAuthor)
				.WithName("AddNewAuthor")
				.AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
				.Produces(401)
				.Produces <ApiResponse<AuthorItem>>();

			// set athor picture
			routeGroupBuilder.MapPost("/{id:int}/pictures", SetAuthorPicture)
				.WithName("SetAuthorPicture")
				.Accepts<IFormFile>("multipart/form-data")
				.Produces(401)
				.Produces<ApiResponse<string>>();


			// update author
			routeGroupBuilder.MapPut("/{id:int}", UpdateAuthor)
				.WithName("UpdateAuthor")
				.Produces(401)
				.Produces<ApiResponse<string>>();


			routeGroupBuilder.MapDelete("/{id:int}", DeleteAuthor)
				.WithName("DeleteAuthor")
				.Produces(401)
				.Produces<ApiResponse<string>>();

			return app;
		}

		private static async Task<IResult> GetAuthors(
			[AsParameters] AuthorFilterModel model,
			IAuthorRepository authorRepository
			)
		{
			var authorList = await authorRepository.GetPagedAuthorsAsync(model, model.Name);

			var paginationResult = new PaginationResult<AuthorItem>(authorList);
			return Results.Ok(ApiResponse.Success(paginationResult));
		}

		// get top author most posts
		private static async Task<IResult> GetTopAuthor(
			int limit,
			IAuthorRepository authorRepository
			)
		{
			var author = await authorRepository.GetTopAuthorMostPosts(limit);
			return Results.Ok(ApiResponse.Success(author));
		}

		// get author details

		private static async Task<IResult> GetAuthorDetails(
			int id, IAuthorRepository authorRepository, IMapper mapper
			)
		{
			var author = await authorRepository.GetCachedAuthorByIdAsync(id);
			return author == null
				? Results.Ok(ApiResponse.Fail(
					HttpStatusCode.NotFound, $"Không tìm thấy tác giả có mã số {id}" ))
				: Results.Ok(ApiResponse.Success(mapper.Map<AuthorItem>(author)));

		}

		//// get post by authod id
		private static async Task<IResult> GetPostsByAuthorId(
			int id,
			[AsParameters] PagingModel pagingModel,
			IBlogRepository blogRepository
			)
		{
			var postQuery = new PostQuery()
			{
				AuthorId = id,
				PublishedOnly = true
			};

			var postList = await blogRepository.GetPagePostsAsync(postQuery, pagingModel,
				posts => posts.ProjectToType<PostDto>());

			var paginationResult = new PaginationResult<PostDto>(postList);

			return Results.Ok(ApiResponse.Success(paginationResult));
		}

		// get post by author slug
		private static async Task<IResult> GetPostsByAuthorSlug(
			[FromRoute] string slug,
			[AsParameters] PagingModel pagingModel,
			IBlogRepository blogRepository)
		{
			var postQuery = new PostQuery()
			{
				AuthorSlug = slug,
				PublishedOnly = true
			};

			var postList = await blogRepository.GetPagePostsAsync(
				postQuery, pagingModel,
				posts => posts.ProjectToType<PostDto>());

			var paginationResult = new PaginationResult<PostDto>(postList);

			return Results.Ok(ApiResponse.Success(paginationResult));
		}

		// add author
		private static async Task<IResult> AddAuthor(
			AuthorEditModel model,
			IAuthorRepository authorRepository,
			IMapper mapper)
		{
			if (await authorRepository.IsAuthorSlugExistedAsync(0, model.UrlSlug))
			{
				//return Results.Conflict($"Slug '{model.UrlSlug}' đã được sử dụng");
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
					$"Slug '{model.UrlSlug}' đã được sử dụng"));
				
			}

			var author = mapper.Map<Author>(model);
			await authorRepository.AddOrUpdateAsync(author);

			//return Results.CreatedAtRoute("GetAuthorsById", 
			//	new { author.Id }, 
			//	mapper.Map<AuthorItem>(author));

			return Results.Ok(ApiResponse.Success(
				mapper.Map<AuthorItem>(author), HttpStatusCode.Created));
		}


		// set author picture
		private static async Task<IResult> SetAuthorPicture(
			int id, IFormFile imageFile,
			IAuthorRepository authorRepository,
			IMediaManager mediaManager
			)
		{
			var imageUrl = await mediaManager.SaveFileAsync(
				imageFile.OpenReadStream(),
				imageFile.FileName, 
				imageFile.ContentType);

			if (string.IsNullOrWhiteSpace(imageUrl))
			{
				//return Results.BadRequest("Không lưu được tập tin");
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tập tin"));
			}
			await authorRepository.SetImageUrlAsync(id, imageUrl);
			return Results.Ok(ApiResponse.Success(imageFile));
		}


		// update author
		private static async Task<IResult> UpdateAuthor(
			int id, AuthorEditModel model,
			IValidator<AuthorEditModel> validator,
			IAuthorRepository authorRepository,
			IMapper mapper)
		{
			var validationResult = await validator.ValidateAsync(model);
			if (!validationResult.IsValid)
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest,validationResult));
			}

			if (await authorRepository.IsAuthorSlugExistedAsync(id, model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(
					HttpStatusCode.Conflict,$"Slug '{model.UrlSlug}' đã được sử dụng"));
			}
			var author = mapper.Map<Author>(model);
			author.Id = id;

			return await authorRepository.AddOrUpdateAsync(author)
				? Results.Ok(ApiResponse.Success("Author is updated", HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find author"));
		}

		private static async Task<IResult> DeleteAuthor(
			int id, IAuthorRepository authorRepository)
		{
			return await authorRepository.DeleteAuthorAsync(id)
				? Results.Ok(ApiResponse.Success("Author is deleted", HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find author"));
		}
	}
}
