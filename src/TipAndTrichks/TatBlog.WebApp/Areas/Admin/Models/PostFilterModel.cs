﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Globalization;

namespace TatBlog.WebApp.Areas.Admin.Models
{
	public class PostFilterModel
	{
		[DisplayName("Từ khoá")]
		public string Keyword { get; set; }

		[DisplayName("Tác giả")]
		public int? AuthorsId { get; set; }

		[DisplayName("Chủ đề")]
		public int? CategoriesId { get; set; }
		[DisplayName("Chưa xuất bản")]
		public bool NotPublished { get; set; }
		[DisplayName("Năm")]
		public int? PostedYear { get; set; }

		[DisplayName("Tháng")]
		public int? PostedMonths { get; set; }


		public IEnumerable<SelectListItem> AuthorList { get; set; }
		public IEnumerable<SelectListItem> CategoryList { get; set; }
		public IEnumerable<SelectListItem> MonthList { get; set; }

		public PostFilterModel() { 
			MonthList = Enumerable.Range(1, 12).Select(m => new SelectListItem()
			{
				Value = m.ToString(),
				Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m)
			}).ToList();
		}
		


	}
}
