using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models
{
	public class CategoryFilterModel
	{
		[DisplayName("Từ khoá")]
		public string Keyword { get; set; }

		public bool ShowOnMenu { get; set; } = false;

	}
}
