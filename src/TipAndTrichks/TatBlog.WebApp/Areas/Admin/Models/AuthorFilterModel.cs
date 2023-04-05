using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Globalization;

namespace TatBlog.WebApp.Areas.Admin.Models
{
	public class AuthorFilterModel
	{
		[DisplayName("Mã tác giả")]
		public int AuthorId { get; set; }
		
		[DisplayName("Tên tác giả")]
		public string FullName { get; set; }
		[DisplayName("Từ khoá")]
		public string Keyword { get; set; }

		[DisplayName("Năm tham gia")]
		public int? JoinYear { get; set; }

		[DisplayName("Tháng tham gia")]
		public int? JoinMonth { get; set; }


		// display list months
		public IEnumerable<SelectListItem> MonthsList { get; set; }
		public AuthorFilterModel()
		{
			MonthsList = Enumerable.Range(1, 12).Select(m => new SelectListItem()
			{
				Value = m.ToString(),
				Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m)
			}).ToList();
		}


	}
}
