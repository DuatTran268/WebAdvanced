using Mapster;
using MapsterMapper;
using System.Net;
using System.Runtime.CompilerServices;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
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
			var posts = await blogRepository.GetCachedPostByIdAsync(id);

			var postQuery = mapper.Map<PostDetail>(posts);

			return postQuery == null
				? Results.Ok(ApiResponse.Fail(
					HttpStatusCode.NotFound, $"Không tìm thấy posts có mã số {id}"))
				: Results.Ok(ApiResponse.Success(mapper.Map<PostDto>(postQuery)));

		}




	}
}
