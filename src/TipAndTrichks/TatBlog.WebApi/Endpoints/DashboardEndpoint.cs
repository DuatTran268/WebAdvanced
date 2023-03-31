using System.Reflection.Metadata.Ecma335;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models.Dashboard;

namespace TatBlog.WebApi.Endpoints
{
	public static class DashboardEndpoint
	{
		public static WebApplication MapDashBoardEndpoint(this WebApplication app)
		{
			var routeGroupBuilder = app.MapGroup("/api/dasboards");
			routeGroupBuilder.MapGet("/", GetInfoDashboard)
				.WithName("GetInfoDashboard")
				.Produces<DashboardModel>();
			return app;
		}

		private static async Task<IResult> GetInfoDashboard(
			IBlogRepository blogRepository,
			IAuthorRepository authorRepository)
		{
			var result = new DashboardModel()
			{
				PostCount = await blogRepository.PostCountAsync(),
				CountPostUnPublish = await blogRepository.PostCountNonPublicAsync(),
				CountAuthor = await authorRepository.CountAuthorAsync(),
				CountCategory = await blogRepository.CountCategoryAsync(),
			};

			return Results.Ok(result);
		}



	}
}
