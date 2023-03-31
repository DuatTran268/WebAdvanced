namespace TatBlog.WebApi.Models.Post
{
	public class PostEditModel
	{
		public string Title { get; set; }
		public string UrlSlug { get; set; }
		public string ShortDescription { get; set; }
		public string Desciption { get; set; }
		public string Meta { get; set; }
		public int AuthorId { get; set; }
		public int CategoryId { get; set; }

		public string SelectTags { get; set; }

		public List<string> GetSelectedTags()
		{
			return (SelectTags ?? "")
			  .Split(new[] { ',', ';', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
			  .ToList();
		}
	}
}
