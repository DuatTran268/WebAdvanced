using MapsterMapper;
using System.Net;
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

			// get tag item
			routeGroupBuilder.MapGet("/{id:int}", GetTagDetails)
			.WithName("GetTagDetails")
			.Produces<ApiResponse<TagItem>>();

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


		// get tag details

		private static async Task<IResult> GetTagDetails(
			int id, IBlogRepository blogRepository, IMapper mapper
			)
		{
			var tags = await blogRepository.GetCachedTagByIdAsync(id);
			return tags == null
				? Results.Ok(ApiResponse.Fail(
					HttpStatusCode.NotFound, $"Không tìm thấy tác giả có mã số {id}"))
				: Results.Ok(ApiResponse.Success(mapper.Map<TagItem>(tags)));

		}
	}
}
