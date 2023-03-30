using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Models.Tag;

namespace TatBlog.WebApi.Endpoints
{
	public static class TagEndpoint
	{
		public static WebApplication MapTagEndpoints(this WebApplication app)
		{
			var routeGroupBuilder = app.MapGroup("/api/tags");


			routeGroupBuilder.MapGet("/", GetTags)
				.WithName("GetTags")
				.Produces<ApiResponse<PaginationResult<TagItem>>>();

			return app;
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
	}
}
