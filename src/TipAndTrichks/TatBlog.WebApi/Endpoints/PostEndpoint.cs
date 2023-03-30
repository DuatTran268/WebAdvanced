using Mapster;
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
			routeGroupBuilder.MapGet("/", GetPosts)
			.WithName("GetPosts")
			.Produces<ApiResponse<PaginationResult<CategoryItem>>>();
			return app;
		}


		private static async Task<IResult> GetPosts(int? id,
			[AsParameters] PostFilterModel model, IBlogRepository blogRepository)
		{
			var postList = await blogRepository.GetPagedPostsAsync(model, model.Keyword);
			var paginationResult = new PaginationResult<PostItem>(postList);

			return Results.Ok(ApiResponse.Success(paginationResult));
		}





	}
}
