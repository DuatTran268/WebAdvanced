using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
	public class AuthorQuery
	{
		public int Id { get; set; }
		public string Keyword { get; set; }
		public int JoinMonth { get; set; }
		public int JoinYear { get; set; }
	}
}
