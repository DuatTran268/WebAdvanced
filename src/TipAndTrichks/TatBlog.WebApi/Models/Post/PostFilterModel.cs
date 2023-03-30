namespace TatBlog.WebApi.Models.Post
{
	public class PostFilterModel :PagingModel
	{
		public string Keyword { get; set; }
		public int? CategoryId { get; set; }
		public int? AuthorId { get; set; }
		public DateTime? PostMonth { get; set; }
		public DateTime? PostYear { get; set; }

	}
}
