using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models
{
	public class AuthorFilterModel
	{
		[DisplayName("Mã tác giả")]
		public int AuthorId { get; set; }
		
		[DisplayName("Tên tác giả")]
		public string FullNames { get; set; }
		[DisplayName("Từ khoá")]
		public string Keyword { get; set; }

		[DisplayName("Năm tham gia")]
		public int Year { get; set; }
		[DisplayName("Tháng tham gia")]
		public int Month { get; set; }


		public IEnumerable<SelectListItem> MonthsList { get; set; }

	}
}
