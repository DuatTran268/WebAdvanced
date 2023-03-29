using MapsterMapper;
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


		// get categories details
		routeGroupBuilder.MapGet("/{id:int}", GetCategoriesDetailsById)
			.WithName("GetCategoriesDetailsById")
			.Produces<CategoryItem>()
			.Produces(404);
		return app;
	}


	private static async Task<IResult> GetCategoies(
		[AsParameters] CategoryFilterModel model, IBlogRepository blogRepository)
	{
		var categoryList = await blogRepository.GetPagedCategoryAsync(model, model.Name);

		var paginationResult = new PaginationResult<CategoryItem>(categoryList);

		return Results.Ok(paginationResult);

	}

	// get category detail by Id
	private static async Task<IResult> GetCategoriesDetailsById(
		int id, IBlogRepository blogRepository, IMapper mapper
		)
	{
		var category = await blogRepository.GetCachedCategoryByIdAsync(id);
		return category == null
			? Results.NotFound($"Không tìm thấy category có mã số {id}")
			:Results.Ok(mapper.Map<CategoryItem>(category));
	}

	



}
