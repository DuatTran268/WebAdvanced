using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SlugGenerator;
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

			////get post filter
			routeGroupBuilder.MapGet("/get-posts-filter", GetFilteredPosts)
				.WithName("GetFilteredPost")
				.Produces<ApiResponse<PostDto>>();

			// filter category
			routeGroupBuilder.MapGet("/get-filter", GetFilter)
				.WithName("GetFilter")
				.Produces<ApiResponse<PostFilterModel>>();



			//routeGroupBuilder.MapPost("/", AddPost)
			//.WithName("AddPost")
			//.Accepts<PostEditModel>("multipart/form-data")
			//.Produces(401)
			//.Produces<ApiResponse<PostItem>>();

			return app;
		}


		private static async Task<IResult> GetCategories2(
		IBlogRepository blogRepository)
		{
			var cateList = await blogRepository.GetCategoryAsync();
			return Results.Ok(ApiResponse.Success(cateList));
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
			var postList = await blogRepository.GetPostSlugAsync(0, 0, slug);

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

		// get filter
		private static async Task<IResult> GetFilter(
			IAuthorRepository authorRepository, IBlogRepository blogRepository)
		{
			var model = new PostFilterModel()
			{
				AuthorList = (await authorRepository.GetAuthorsAsync())
				.Select(a => new SelectListItem()
				{
					Text = a.FullName,
					Value = a.Id.ToString()
				}),
				CategoryList = (await blogRepository.GetCategoryAsync())
				.Select(c => new SelectListItem()
				{
					Text = c.Name,
					Value = c.Id.ToString()
				})
			};

			return Results.Ok(ApiResponse.Success(model));
		}

		private static async Task<IResult> GetFilteredPosts(
			[AsParameters] PostFilterModel model,
			IMapper mapper,
			IBlogRepository blogRepository)
		{
			var postQuery = mapper.Map<PostQuery>(model);
			var postsList = await blogRepository.GetPagePostsAsync(postQuery, model, posts =>
			posts.ProjectToType<PostDto>());
			var paginationResult = new PaginationResult<PostDto>(postsList);
			return Results.Ok(ApiResponse.Success(paginationResult));
		}


		// add post
		private static async Task<IResult> AddPost(HttpContext context,
				IBlogRepository blogRepository, IMapper mapper,
				IMediaManager mediaManager)
		{
			var model = await PostEditModel.BindAsync(context); var slug = model.Title.GenerateSlug();
			if (await blogRepository.IsPostSlugExistedAsync(model.Id, slug))
			{
				return Results.Ok(ApiResponse.Fail(
				HttpStatusCode.Conflict, $"Slug '{slug}' đã được sử dụng cho bài viết khác"));
			}

			var post = model.Id > 0 ? await blogRepository.GetPostByIdAsync(model.Id) : null;

			if (post == null)
			{
				post = new Post()
				{
					PostedDate = DateTime.Now
				};
			}

			post.Title = model.Title; post.AuthorId = model.AuthorId; post.CategoryId = model.CategoryId;
			post.ShortDescription = model.ShortDescription; post.Description = model.Description; post.Meta = model.Meta;
			post.Published = model.Published; post.ModifiedDate = DateTime.Now; post.UrlSlug = model.Title.GenerateSlug();

			if (model.ImageFile?.Length > 0)
			{
				string hostname =
				$"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/",
				uploadedPath = await
				mediaManager.SaveFileAsync(model.ImageFile.OpenReadStream(), model.ImageFile.FileName,
				model.ImageFile.ContentType);

				if (!string.IsNullOrWhiteSpace(uploadedPath))
				{
					post.ImageUrl = uploadedPath;
				}
			}

			await blogRepository.CreateOrUpdatePostAsync(post, model.GetSelectedTags());

			return Results.Ok(ApiResponse.Success(mapper.Map<PostItem>(post), HttpStatusCode.Created));
		}


	}
}
