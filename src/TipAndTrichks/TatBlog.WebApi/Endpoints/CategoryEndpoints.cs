using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models.Category;

namespace TatBlog.WebApi.Endpoints;

public static class CategoryEndpoints
{
	public static WebApplication MapCategoryEndpoint(this WebApplication app)
	{
		// getcategory
		var routeGroupBuilder = app.MapGroup("/api/categorys");
		routeGroupBuilder.MapGet("/", GetCategoies)
			.WithName("GetCategoies")
			.Produces<PaginationResult<CategoryItem>>();

		return app;
	}


	private static async Task<IResult> GetCategoies(
		[AsParameters] CategoryFilterModel model, IBlogRepository blogRepository)
	{
		var categoryList = await blogRepository.GetPagedCategoryAsync(model, model.Name);

		var paginationResult = new PaginationResult<CategoryItem>(categoryList);

		return Results.Ok(paginationResult);


	}

}
