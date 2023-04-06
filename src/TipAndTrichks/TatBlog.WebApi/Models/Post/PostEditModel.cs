using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.OpenApi.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TatBlog.WebApi.Models.Post
{
	public class PostEditModel
	{
		public int Id { get; set; }


		[DisplayName("Tiêu đề")]
		[Required(ErrorMessage = "Tiêu đề không được đề trống")]
		[MaxLength(500, ErrorMessage ="Tiêu đề tối đa 500 ký tự")]
		public string Title { get; set; }


		[DisplayName("Giới thiệu")]
		[Required]
		public string ShortDescription { get; set; }


		[DisplayName("Nội dung")]
		[Required]
		public string Description { get; set; }


		[DisplayName("Metadata")]
		[Required]
		public string Meta { get; set; }

		[DisplayName("Chọn hình ảnh")]
		public IFormFile ImageFile { get; set; }


		[DisplayName("Hình hiện tại")]
		public string ImageUrl { get; set; }

		public bool Published { get; set; }

		[DisplayName("Tác giả")]
		[Required]
		public int AuthorId { get; set; }


		[DisplayName("Chủ đề")]
		[Required]
		public int CategoryId { get; set; }


		[DisplayName("Từ khoá (mỗi từ 1 dòng)")]
		[Required]
		public string SelectTags { get; set; }
		public IEnumerable<SelectListItem> AuthorList { get; set; }
		public IEnumerable<SelectListItem> CategoryList { get; set; }
		
		
		public List<string> GetSelectedTags()
		{
			return (SelectTags ?? "")
			  .Split(new[] { ',', ';', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
			  .ToList();
		}

		public static async ValueTask<PostEditModel> BindAsync(HttpContext context)
		{
			var form = await context.Request.ReadFormAsync();
			return new PostEditModel()
			{
				ImageFile = form.Files["ImageFile"],
				Id = int.Parse(form["Id"]),
				Title = (form["Title"]),
				ShortDescription = (form["ShortDescription"]),
				Description = (form["Description"]),
				Meta = form["Meta"],
				Published = form["Published"] != "false",
				CategoryId = int.Parse(form["CategoryId"]),
				AuthorId = int.Parse(form["AuthorId"]),
				SelectTags = form["SelectTags"]
			};
		}
	}
}
