namespace TatBlog.WebApi.Models.Post
{
	public class PostEditModel : PagingModel
	{
		public string Title { get; set; }
		public string UrlSlug { get; set; }
		public string ShortDescription { get; set; }
		public string Desciption { get; set; }
		public string Meta { get; set; }

	}
}
