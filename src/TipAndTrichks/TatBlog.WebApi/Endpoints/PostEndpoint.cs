using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Runtime.CompilerServices;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Models.Category;
using TatBlog.WebApi.Models.Post;

namespace TatBlog.WebApi.Endpoints
{
	public static class PostEndpoint
	{
		public static WebApplication MapPostEndpoint(this WebApplication app)
		{
			var routeGroupBuilder = app.MapGroup("/api/posts");
			//routeGroupBuilder.MapGet("/", GetPosts)
			//.WithName("GetPosts")
			//.Produces<ApiResponse<PaginationResult<CategoryItem>>>();
			routeGroupBuilder.MapGet("/", GetPosts)
			.WithName("GetPosts")
			.Produces<ApiResponse<PaginationResult<PostDto>>>();

			routeGroupBuilder.MapGet("/{id:int}", GetPostsDetailsById)
				.WithName("GetPostsDetailsById")
				.Produces<ApiResponse<PostDto>>();

			// set avata image
			routeGroupBuilder.MapPost("/{id:int}/avatar", SetAvatarPost)
			.WithName("SetAvatarPost")
			.Accepts<IFormFile>("multipart/form-data")
			.Produces(401)
			.Produces<ApiResponse<string>>();

			// delete post

			routeGroupBuilder.MapDelete("/{id:int}", DeletePost)
				.WithName("DeletePost")
				.Produces(401)
				.Produces<ApiResponse<string>>();

			// get N post top reader
				routeGroupBuilder.MapGet("featured/{limit:int}", GetNPostTopReader)
				.WithName("GetNPostTopReader")
				.Produces<ApiResponse<IList<PostDto>>>();

			// get random post 
				routeGroupBuilder.MapGet("random/{limit:int}", GetNRandomPostList)
				.WithName("GetNRandomPostList")
				.Produces<ApiResponse<IList<PostDto>>>();

			// get post recent 
			routeGroupBuilder.MapGet("archives/{limit:int}", GetPostRecentMonth)
			.WithName("GetPostRecentMonth")
			.Produces<ApiResponse<IList<CountPostMonthly>>>();

			// get post details
			routeGroupBuilder.MapGet("/byslug/{slug:regex(^[a-z0-9_-]+$)}", GetPostsDetailBySlug)
			.WithName("GetPostsDetailBySlug")
			.Produces<ApiResponse<PostDetail>>();

			// add new post
			routeGroupBuilder.MapPost("/", AddNewPost)
				.WithName("AddNewPost")
				.AddEndpointFilter<ValidatorFilter<PostEditModel>>()
				.Produces(401)
				.Produces<ApiResponse<PostDto>>();

			// update category
			routeGroupBuilder.MapPut("/{id:int}", UpdatePost)
				.WithName("UpdatePost")
				.AddEndpointFilter<ValidatorFilter<PostEditModel>>()
				.Produces(401)
				.Produces<ApiResponse<PostDto>>();
			return app;
		}


		private static async Task<IResult> GetPosts([AsParameters] PostFilterModel model, 
			IBlogRepository blogRepository, IMapper mapper)
		{
			var postQuery = mapper.Map<PostQuery>(model);


			var postList = await blogRepository.GetPagePostsAsync(postQuery, model,
				posts => posts.ProjectToType<PostDto>());

			var paginationResult = new PaginationResult<PostDto>(postList);

			return Results.Ok(ApiResponse.Success(paginationResult));

		}

		private static async Task<IResult> GetPostsDetailsById(
		int id, IBlogRepository blogRepository, IMapper mapper
		)
		{
			var posts = await blogRepository.GetCachedPostByIdAsync(id, true);

			var postQuery = mapper.Map<PostDetail>(posts);

			return postQuery == null
				? Results.Ok(ApiResponse.Fail(
					HttpStatusCode.NotFound, $"Không tìm thấy posts có mã số {id}"))
				: Results.Ok(ApiResponse.Success(mapper.Map<PostDto>(postQuery)));

		}

		private static async Task<IResult> SetAvatarPost(
			int id, IFormFile imageFile, IBlogRepository blogRepository, IMediaManager mediaManager)
		{
			var imageUrl = await mediaManager.SaveFileAsync(
				imageFile.OpenReadStream(), imageFile.FileName, imageFile.ContentType);
			if (string.IsNullOrWhiteSpace(imageUrl))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tập tin"));
			}

			await blogRepository.SetImageUrlAsync(id, imageUrl);

			return Results.Ok(ApiResponse.Success(imageFile));
			
		}

		// delete post
		private static async Task<IResult> DeletePost(
			int id, IBlogRepository blogRepository)
		{
			return await blogRepository.DeletePostById(id)
				? Results.Ok(ApiResponse.Success("Post is deleted ", HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find post"));
		}

		// get n post top reader
		private static async Task<IResult> GetNPostTopReader(int limit,
			IBlogRepository blogRepository, ILogger<IResult> logger)
		{
			var postsTopReader = await blogRepository.GetNPostTopReaderAsync(limit,
				p => p.ProjectToType<PostDto>());
			return Results.Ok(ApiResponse.Success(postsTopReader));
		}



		// get random post list
		public static async Task<IResult> GetNRandomPostList(int limit,
			IBlogRepository blogRepository, ILogger<IResult> logger)
		{
			var postRandom = await blogRepository.GetNRandomPostAsync(
				limit, p => p.ProjectToType<PostDto>());

			return Results.Ok(ApiResponse.Success(postRandom));

		}

		public static async Task<IResult> GetPostRecentMonth(int limit,
			IBlogRepository blogRepository, ILogger<IResult> logger)
		{
			var postRecentMonth = await blogRepository.CountPostInMonth(limit);

			return Results.Ok(ApiResponse.Success(postRecentMonth));
		}

		// get post details by slug
		private static async Task<IResult> GetPostsDetailBySlug(
			[FromRoute] string slug,
			IBlogRepository blogRepository,
			IMapper mapper)
		{
			var postList = await blogRepository.GetPostSlugAsync(0, 0,slug);

			return postList == null
				? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"No find post '{slug}'"))
				: Results.Ok(ApiResponse.Success(mapper.Map<PostDetail>(postList)));
		}

		// add post
		private static async Task<IResult> AddNewPost(
			PostEditModel model,
			IAuthorRepository authorRepository,
			IBlogRepository blogRepository,
			IMapper mapper,
			IMediaManager mediaManager)
		{

			if (await blogRepository.IsPostSlugExistedAsync(0, model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
					$"Slug '{model.UrlSlug}' đã được sử dụng"));

			}
			if (await authorRepository.GetAuthorByIdAsync(model.AuthorId) == null)
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
					$"Không tìm thấy tác giả có id '{model.AuthorId}'"));
			}

			if (await blogRepository.GetCategoryByIdAsync(model.CategoryId) == null)
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
					$"Không tìm thấy chủ đề có id '{model.CategoryId}'"));
			}

				var post = mapper.Map<Post>(model);
			post.PostedDate = DateTime.Now;

			await blogRepository.CreateOrUpdatePostAsync(post, model.GetSelectedTags());

			return Results.Ok(ApiResponse.Success(mapper.Map<PostDto>(post), 
				HttpStatusCode.Created));
		}



		//update post
		private static async Task<IResult> UpdatePost(
			int id,
			PostEditModel model,
			IAuthorRepository authorRepository,
			IBlogRepository blogRepository,
			IMapper mapper,
			IMediaManager mediaManager)
		{

			var post = await blogRepository.GetPostByIdAsync(id);
			if (post == null)
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
					$"Không tìm thấy id '{id}' của bài viết"));

			}
			if (await authorRepository.GetAuthorByIdAsync(model.AuthorId) == null)
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
					$"Không tìm thấy tác giả có id '{model.AuthorId}'"));
			}

			if (await blogRepository.GetCategoryByIdAsync(model.CategoryId) == null)
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
					$"Không tìm thấy chủ đề có id '{model.CategoryId}'"));
			}
			if (await blogRepository.IsPostSlugExistedAsync(0, model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
					$"Slug '{model.UrlSlug}' đã được sử dụng"));
			}

			mapper.Map(model, post);
			post.Id = id;
			post.ModifiedDate = DateTime.Now;

			return await blogRepository.CreateOrUpdatePostAsync(post, model.GetSelectedTags())
			   ? Results.Ok(ApiResponse.Success($"Thay đổi thành công bài viết có id = {id}"))
			   : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bài viết có id = {id}"));
		}


	}
}
