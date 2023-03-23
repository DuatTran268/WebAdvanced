namespace TatBlog.WebApp.Areas.Admin.Models
{
	public class AuthorEditModel
	{
		public int Id { get; set; }
		public string FullNames { get; set; }
		public string UrlSlug { get; set; }
		public string Email { get; set; }
		public string Note { get; set; }
		public string ImageUrl { get; set; }
		public IFormFile ImageFile { get; set; }

	}
}
