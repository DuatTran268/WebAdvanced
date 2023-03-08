using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Controllers
{
    public class BlogController :Controller
    {
        private readonly IBlogRepository _blogRepository;
        //public IActionResult Index()
        //{
        //    ViewBag.CurrentTime = DateTime.Now.ToString("HH:mm:ss");
        //    return View();
        //}

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

        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        // action xuwr lys http request đến trang chủ của ứng dụng web hoặc tìm kiếm bài viết theo từ khoá
        public async Task<IActionResult> Index(
            [FromQuery(Name ="k")] string keyword = null,
            [FromQuery(Name ="p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 2
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
            //IPagingParams pagingParams = new PagingParams()
            //{
            //    PageNumber = pageNumber,
            //    PageSize = pageSize,
            //    SortColumn = "PostedDate",
            //    SortOrder = "DESC"
            //};

            var postList = await _blogRepository.GetPagePostAsync(postQuery, pageNumber, pageSize);
            //var postList = await _blogRepository.GetPagePostsAsync(postQuery, pagingParams);
            // lưu lại điều kiện truy vấn
            ViewBag.PostQuery = postQuery;

            return View(postList);

        }
    }
}
