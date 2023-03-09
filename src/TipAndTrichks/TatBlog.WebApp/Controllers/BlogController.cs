using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Controllers
{
    public class BlogController :Controller
    {
        private readonly IBlogRepository _blogRepository;
        //private readonly IAuthorRepository _authorRepository;
		//public IActionResult Index()
		//{
		//    ViewBag.CurrentTime = DateTime.Now.ToString("HH:mm:ss");
		//    return View();
		//}
		public BlogController(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;
            //_authorRepository = authorRepository;
		}
		public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Rss()
        {
            return Content("Nội dung sẽ được cập nhật sau");
        }




        // action xử lý http request đến trang chủ của ứng dụng web hoặc tìm kiếm bài viết theo từ khoá
        public async Task<IActionResult> Index(
            [FromQuery(Name ="k")] string keyword = null,
            [FromQuery(Name ="p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 3
            )
        {
            // tạo đối tượng chứa các điều kiện truy vấn
            var postQuery = new PostQuery()
            {
                // chỉ lấy những bài viết có trạng thái Published
                PublishedOnly = true,

                // tìm bài viết theo từ khoá
                Keyword = keyword
            };

            // truy vấn các bài viết theo điều kiện đã tạo
            IPagingParams pagingParams = new PagingParams()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortColumn = "PostedDate",
                SortOrder = "DESC"
            };

            //var postList = await _blogRepository.GetPagePostAsync(postQuery, pageNumber, pageSize);
            var postList = await _blogRepository.GetPagePostsAsync(postQuery, pagingParams);
            // lưu lại điều kiện truy vấn
            ViewBag.PostQuery = postQuery;

            return View(postList);

        }


        // action category 

        public async Task<IActionResult> Category (
            string slug,
			[FromQuery(Name = "p")] int pageNumber = 1,
			[FromQuery(Name = "ps")] int pageSize = 3)
        {
            var category = await _blogRepository.GetCategoryBySlugAsync(slug);
            var postQuery = new PostQuery()
            {
				PublishedOnly = true,
				CategorySlug = slug,
            };


			IPagingParams pagingParams = CreatePagingParam(pageNumber, pageSize);
			var posts = await _blogRepository
			  .GetPagePostsAsync(postQuery, pagingParams);

			// lưu lại bảng truy vấn 
			ViewBag.PostQuery = postQuery;
            ViewBag.Title = $"Bài viết của chủ đề '{category.Name}'";

            return View("Index",posts);
        }

        public async Task<IActionResult> Author(
            string slug,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 3)
        {
            var author = await _blogRepository.GetAuthorBySlugAsync(slug);
			var postQuery = new PostQuery()
			{
				PublishedOnly = true,
				AuthorSlug = slug,
			};

			IPagingParams pagingParams = CreatePagingParam(pageNumber, pageSize);
			var posts = await _blogRepository
			  .GetPagePostsAsync(postQuery, pagingParams);

			// lưu lại bảng truy vấn 
			ViewBag.PostQuery = postQuery;
			ViewBag.Title = $"Bài viết của chủ đề '{author.FullNames}'";

			return View("Index", posts);

		}



        public IPagingParams CreatePagingParam(
            int pageNumber = 1,
            int pageSize = 3,
			 string sortColumn = "PostedDate",
	        string sortOrder = "DESC")
        {
			return new PagingParams()
			{
				PageNumber = pageNumber,
				PageSize = pageSize,
				SortColumn = sortColumn,
				SortOrder = sortOrder
			};
		}
            
            

	}
}
