﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
	public class PostItem
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string ShortDescription { get; set; }
		public string Description { get; set; }
		public string CategoryName { get; set; }
		public int ViewCount { get; set; }
		public string UrlSlug { get; set; }
		public string ImageUrl { get; set; }
		public bool Published { get; set; }
		public DateTime PostDate { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public IList<string> Tags { get; set; }
	}

}
