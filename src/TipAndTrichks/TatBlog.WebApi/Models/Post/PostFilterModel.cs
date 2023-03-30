namespace TatBlog.WebApi.Models.Post
{
	public class PostFilterModel
	{
		public string Keyword { get; set; }
		public string CategorySlug { get; set; }
		public string AuthorSlug { get; set; }
		public DateTime PostDate { get; set; }
		public string PostSlug { get; set; }
	}
}
