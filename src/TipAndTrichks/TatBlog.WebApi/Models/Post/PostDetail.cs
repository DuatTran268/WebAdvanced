using TatBlog.WebApi.Models.Author;
using TatBlog.WebApi.Models.Category;
using TatBlog.WebApi.Models.Tag;

namespace TatBlog.WebApi.Models.Post
{
    public class PostDetail
    {
        // ma bai viet
        public int Id { get; set; }
        // tieu de bai viet
        public string Title { get; set; }
        // mo ta hoac gio thieu ngan ve noi dng
        public string Description { get; set; }
        // metadata
        public string Meta { get; set; }
        // ten dinh danh de tao url slug
        public string UrlSlug { get; set; }

        // duong danh den tap tin hinh anh
        public string ImageUrl { get; set; }
        //so luot xem, doc bai viet
        public int ViewCount { get; set; }
        //ngay gio dang bai
        public DateTime PostedDate { get; set; }
        // ngay gio cap nhat lan cuoi
        public DateTime? ModifiedDate { get; set; }
        // chuyen muc cua bai viet
        public CategoryDto Category { get; set; }

        // tac gia cua bai viet
        public AuthorDto Author { get; set; }

        // danh sach tu khoa cua bai viet
        public IList<TagDto> Tags { get; set; }

    }
}
