using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models
{
	public class AuthorFilterModel
	{
		[DisplayName("Từ khoá")]
		public string Keyword { get; set; }

		[DisplayName("Năm tham gia")]
		public int Year { get; set; }
		[DisplayName("Tháng tham gia")]
		public int Month { get; set; }


		public IEnumerable<SelectListItem> MonthsList { get; set; }

	}
}
